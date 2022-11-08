using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public enum BattleState { START, PLAYERSELECT, ENEMYSELECT, BATTLE, WON, LOST }

public class BattleSystem : Controllable
{
    #region Singleton

    public static BattleSystem instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of BattleSystem found!");
            return;
        }
        instance = this;
    }

    #endregion

    public BattleState state;

    public List<int> spawnLocations;//battlestation to spawn each enemy on

    public List<Transform> playerBattleStations;
    public List<Transform> enemyBattleStations;

    public PresetBattle defaultBattle;//used for testing when starting in battle scene

    public List<GameObject> playerGOs;
    public List<PlayerUnit> team;
    public List<GameObject> enemyGOs;
    public List<EnemyUnit> enemies;

    public List<Action> actionsToUse = new List<Action>();

    public TextMeshProUGUI dialogueText;

    public TextMeshProUGUI turnNumberText;
    public int turnNumber = 0;

    public List<BattleHUD> playerHUDs;

    bool playerCanAbsorb = false;
    bool absorbColor = false;

    // Start is called before the first frame update
    void Start()
    {
        //This is the first thing that has control in battles
        //It gets control in START ENEMYSELECT ATTACK WON and LOST battle states
        //The only thing the player can do when this has control is (sometimes) speedup/skip text or press a key to end the battle in WON and LOST
        hasControl = true;

        state = BattleState.START;
        StartCoroutine(setupBattle());
    }

    void Update()
    {
        //check if player wants to absorb an attack
        if (playerCanAbsorb && state == BattleState.BATTLE)
        {
            Debug.Log("Press Space to Absorb!");
            if (Input.GetKey(KeyCode.Space))
            {
                //absorb the color
                absorbColor = true;
            }
        }
    }

    IEnumerator setupBattle()
    {
        if (BattleInitializer.instance.getIsBattleSet())
        {
            //get info from BattleInitializer
            List<GameObject> enemyPrefabs = BattleInitializer.instance.getBattle().getEnemyPrefabs();
            //spawn the enemies (they are prefabs)
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                GameObject enemy = enemyBattleStations[i].GetComponent<EnemyBattleStation>().fillStation(enemyPrefabs[i]);
                enemyGOs.Add(enemy);
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                enemyUnit.updateColor();
                enemyUnit.scaleSprite();
                enemies.Add(enemyUnit);

                enemy.SetActive(true);
            }

            playerGOs = BattleInitializer.instance.getBattle().getPlayerGOs();
            for (int i = 0; i < playerGOs.Count; i++)
            {
                GameObject player = playerGOs[i];
                player.transform.position = playerBattleStations[i].transform.position;
                PlayerUnit playerUnit = player.GetComponent<PlayerUnit>();
                playerUnit.updateColor();
                playerUnit.scaleSprite();
                team.Add(playerUnit);

                player.SetActive(true);
            }

        }
        else
        {//no battle is set, use default battle (testing only)
            //get info from defaultBattle
            List<GameObject> enemyPrefabs = defaultBattle.getEnemyPrefabs();
            List<GameObject> playerPrefabs = defaultBattle.getPlayerGOs();//In this specific case, the preset battle contains player prefabs
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                GameObject enemy = enemyBattleStations[i].GetComponent<EnemyBattleStation>().fillStation(enemyPrefabs[i]);
                enemyGOs.Add(enemy);
                EnemyUnit enemyUnit = enemy.GetComponent<EnemyUnit>();
                enemyUnit.updateColor();
                enemyUnit.scaleSprite();
                enemies.Add(enemyUnit);

                enemy.SetActive(true);
            }

            for (int i = 0; i < playerPrefabs.Count; i++)
            {
                GameObject player = Instantiate(playerPrefabs[i], TeamManager.instance.transform);//child of GameController
                player.transform.position = playerBattleStations[i].transform.position;//set position
                playerGOs.Add(player);

                player.transform.position = playerBattleStations[i].transform.position;
                PlayerUnit playerUnit = player.GetComponent<PlayerUnit>();
                playerUnit.updateColor();
                playerUnit.scaleSprite();
                team.Add(playerUnit);

                player.SetActive(true);
            }
        }

        dialogueText.text = "A wild " + enemies[0].unitName + " approaches";


        //activate needed HUDs
        for (int i = 0; i < team.Count; i++)
        {
            playerHUDs[i].gameObject.SetActive(true);
        }
        updateHUDs();//updates all HUDs

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERSELECT;
        playerSelect();
    }

    void playerSelect()
    {
        //update turnNumber
        turnNumber++;
        turnNumberText.text = turnNumber.ToString();

        dialogueText.text = "Choose an action";
        //give control to TurnMenu
        ControlManager.instance.switchControl(TurnMenuControl.instance);
    }

    public void enemySelect()
    {
        state = BattleState.ENEMYSELECT;
        //select enemy's moves one by one
        //load them into global list of moves

        Debug.Log("enemy is selecting moves");

        foreach (EnemyUnit enemy in enemies)
        {
            Action enemyMove = enemy.selectAction();
            if (enemyMove != null)//this move is not a pass
            {
                addAction(enemyMove);
            }
        }
        //change to BATTLE phase
        state = BattleState.BATTLE;
        StartCoroutine(battle());
    }

    IEnumerator battle()
    {
        Debug.Log("The battle is happening!");
        //sort actionsToUse by unit speed
        actionsToUse.Sort(compareActions);

        //call actions one at a time
        foreach (Action action in actionsToUse)
        {
            int targetType = action.getTargetType();

            //used to indicate why a move failed
            bool userDead = false;
            bool insufficientM = false;//can only be true if user is player
            bool allTargetsDead = false;



            //check if moves fails and "alter" it if needed
            //check for user death.
            userDead = action.getUserDead();
            if (!userDead)
            {
                //check for user insufficient M
                insufficientM = action.getInsufficientM();
                if (!insufficientM)
                {
                    switch (targetType)
                    {
                        case 0://no targets
                        case 1://self target
                        default:
                            //we are ready to do the move
                            break;

                        case 2://1 teammate
                        case 4://1 enemy
                        case 6://1 unit
                            //check if target is dead
                            allTargetsDead = action.getAllTargetsDead();
                            if (allTargetsDead)
                            {//find a replacement
                                Unit oldTarget = action.getTargets()[0];
                                action.removeTarget(oldTarget);
                                Unit replacement = findBestTarget(oldTarget);
                                if (replacement != null)
                                {//we found one! replace it in action

                                    action.addTarget(replacement);
                                    allTargetsDead = false;
                                }
                            }
                            break;

                        case 5://all enemies
                        case 3://all teammates
                        case 7://all units
                            //remove dead targets
                            allTargetsDead = action.removeAllDeadTargets();
                            break;
                    }
                }
            }

            //Check all bools and perform move if valid
            if (!userDead && !insufficientM && !allTargetsDead)
            {
                //perform action
                action.updateColor();//sets color to that of user if applicable
                action.performAction();

                //lower currentM of user
                action.spendM();

                //!!!in place of action's animation and text
                //This will probably be a method call in the user and targets starting an animation on sprite.
                dialogueText.text = action.moveCompletedText();
                
                //highlight and do animations
                action.getUser().setHighlight(Highlight.ACTING);
                foreach (Unit target in action.getTargets())
                {
                    target.setHighlight(Highlight.TARGETTED);
                }
                yield return new WaitForSeconds(2f);//animations finish during this pause

                //update HUD
                updateHUDs();

                //living targets select whether to absorb the move if it can be absorbed and performs absorb
                action.removeAllDeadTargets();
                List<Unit> targets = action.getTargets();
                List<EnemyUnit> enemyTargets = getEnemyUnits(targets);
                List<PlayerUnit> playerTargets = getPlayerUnits(targets);
                //enemies
                bool[] enemyUnitsToAbsorb = new bool[enemyTargets.Count];
                if (action.getAbsorbable())
                {
                    //enemies
                    foreach (EnemyUnit enemy in enemyTargets)
                    {
                        if (enemy.absorbAction())
                        {//enemy wants to absorb the action
                            enemy.combineColor(action.getColor());
                            //!!! start animation. I may do this in combineColor()
                        }
                    }
                    //players
                    //!!!
                    if (playerTargets.Count != 0)
                    {
                        playerCanAbsorb = true;
                        //if button is pressed in this time, make absorb status true
                        yield return new WaitForSeconds(2f);
                        if (absorbColor)
                        {//player wants to absorb color
                            playerTargets[0].combineColor(action.getColor());
                        }
                        //reset bools
                        absorbColor = false;
                        playerCanAbsorb = false;
                    }
                }
                
            }
            else if (userDead)
            {
                dialogueText.text = action.getUser().unitName + " is dead and cannot use a move";
                yield return new WaitForSeconds(2f);
            }
            else if (insufficientM)
            {
                dialogueText.text = action.getUser().unitName + " does not have enough magic to use " + action.name;
                yield return new WaitForSeconds(2f);
            }
            else if (allTargetsDead)
            {
                dialogueText.text = action.getUser().unitName + " tried " + action.name + " but there were no targets for the action";
                yield return new WaitForSeconds(2f);
            }
            else
            {
                Debug.Log("How did this happen?");
                yield return new WaitForSeconds(2f);
            }

            //update Highlights
            updateHighlights();
        }

        //clear the previous turn's actions
        clearSkills();

        //get rid of dead enemies (This only happens at the end of each turn since I want enemy revival to be possible)
        //!!!This is not working
        enemies.RemoveAll(enemy => enemy.currentH <= 0);
        foreach (Transform station in enemyBattleStations)
        {
            station.GetComponent<EnemyBattleStation>().deleteIfDead();
        }

        //check if either side has won.
        bool teamIsDead = true;
        foreach (PlayerUnit player in team)
        {
            if (player.currentH > 0)
            {
                teamIsDead = false;
                break;
            }
        }
        bool enemiesAreDead = true;
        foreach (EnemyUnit enemy in enemies)
        {
            if (enemy.currentH > 0)
            {
                enemiesAreDead = false;
                break;
            }
        }

        if (teamIsDead)
        {
            //player lost
            state = BattleState.LOST;
            battleLost();
        }
        else if (enemiesAreDead)
        {
            //player won!
            state = BattleState.WON;
            StartCoroutine(battleWon());
        }
        else
        {
            //change state to PLAYERSELECT
            state = BattleState.PLAYERSELECT;
            playerSelect();
        }
    }

    IEnumerator battleWon()
    {
        dialogueText.text = "you won the battle!";
        yield return new WaitForSeconds(2f);
        TeamManager.instance.deactivatePlayers();//hides all player GOs
        SceneManager.LoadScene("Overworld");
    }

    void battleLost()
    {
        dialogueText.text = "you were defeated.";
    }

    #region battleUtilities
    void updateHUDs()
    {
        for (int i = 0; i < team.Count; i++)
        {
            playerHUDs[i].setHUD(team[i]);
        }
    }

    public void updateHighlights()
    {//sets highlight to DEAD if dead, NONE if alive
        foreach (Unit player in team)
        {
            if (player.currentH > 0)
            {
                player.setHighlight(Highlight.NONE);
            }
            else
            {
                player.setHighlight(Highlight.DEAD);
            }
        }
        foreach (Unit enemy in enemies)
        {
            if (enemy.currentH > 0)
            {
                enemy.setHighlight(Highlight.NONE);
            }
            else
            {
                enemy.setHighlight(Highlight.DEAD);
            }
        }
    }

    public void addAction(Action action)
    {
        actionsToUse.Add(action);
    }

    public void removeAction()
    {
        int count = actionsToUse.Count;
        if (count != 0)
        {
            actionsToUse.RemoveAt(count - 1);
        }
    }

    void clearSkills()
    {
        actionsToUse.Clear();
    }

    //used to sort actions in actionsToUse list
    int compareActions(Action a, Action b)
    {
        if (a == null || b == null)
        {
            return 0;
        }

        int aUserSpeed = a.getUserSpeed();
        int bUserSpeed = b.getUserSpeed();

        return aUserSpeed.CompareTo(bUserSpeed);
    }

    Unit findBestTarget(Unit target)
    {//the target is dead. find the best alternative if there is one. 
        var enemyTarget = target as EnemyUnit;
        if (enemyTarget != null)
        {//target is an enemyUnit
            foreach (EnemyUnit enemy in enemies)
            {
                if (enemy.currentH > 0)
                {
                    return enemy;
                }
            }
        }
        else
        {//target is a playerUnit
            foreach (PlayerUnit player in team)
            {
                if (player.currentH > 0)
                {
                    return player;
                }
            }
        }
        return null;
    }

    List<EnemyUnit> getEnemyUnits(List<Unit> units)
    {
        List<EnemyUnit> enemyUnits = new List<EnemyUnit>();
        foreach (Unit unit in units)
        {
            EnemyUnit enemyUnit = unit as EnemyUnit;
            if (enemyUnit != null)
            {
                enemyUnits.Add(enemyUnit);
            }
        }
        return enemyUnits;
    }
    List<PlayerUnit> getPlayerUnits(List<Unit> units)
    {
        List<PlayerUnit> playerUnits = new List<PlayerUnit>();
        foreach (Unit unit in units)
        {
            PlayerUnit playerUnit = unit as PlayerUnit;
            if (playerUnit != null)
            {
                playerUnits.Add(playerUnit);
            }
        }
        return playerUnits;
    }
    #endregion

    #region damageCalculation

    public int getBaseDamage(Unit attacker, Unit defender, Element attColor)
    {//calculates damage of basic attack by attacker on defender
        //often used in other actions as well
        int attack = attacker.attack;
        int defense = defender.defense;
        Element defColor = defender.color;

        int index = ElementManager.instance.elementDict[attColor].Item6;
        Affinity defAff = defender.weaknesses[index];

        return getBaseDamage(attack, defense, attColor, defColor, defAff);
    }

    public int getBaseDamage(int attack, int defense, Element attColor, Element defColor, Affinity defAff)
    {//used explicitly when one or more arguments is artificially changed for specific action's effect
        double aff = getAff(defAff);//defAff is affinity to attColor
        int same = getSame(attColor, defColor);
        int other = 1;//no use right now

        int damage = (int)Math.Ceiling(Math.Pow(attack, 2) * aff * other *Math.Pow(defense, -1) * Math.Pow(same, -1));
        return damage;
    }

    public double getAff(Affinity defAff)
    {
        switch (defAff)
        {
            case Affinity.WEAK:
                return 2.0;
            case Affinity.NORMAL:
            default:
                return 1.0;
            case Affinity.STRONG:
                return 0.5;
        }
    }

    public int getSame(Element attColor, Element defColor)
    {
        if (attColor == defColor)
        {
            return 3;
        }
        return 1;
    }

#endregion
}
