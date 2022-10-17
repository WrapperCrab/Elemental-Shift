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

    IEnumerator setupBattle()
    {
        if (BattleInitializer.instance.getIsBattleSet())
        {
            //get info from BattleInitializer
            List<GameObject> enemyPrefabs = BattleInitializer.instance.getBattle().getEnemyPrefabs();
            //spawn the enemies (they are prefabs)
            for (int i = 0; i < enemyPrefabs.Count; i++)
            {
                GameObject enemy = Instantiate(enemyPrefabs[i], enemyBattleStations[i]);
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
                GameObject enemy = Instantiate(enemyPrefabs[i], enemyBattleStations[i]);
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
        for (int i=0; i< team.Count; i++)
        {
            playerHUDs[i].gameObject.SetActive(true);
        }
        setHUDs();//updates all HUDs

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

            //bool skipThisMove = false;

            bool actionCompleted = false;




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
            {//perform the action and highlight
                action.getUser().setHighlight(Highlight.ACTING);
                foreach (Unit target in action.getTargets())
                {
                    target.setHighlight(Highlight.TARGETTED);
                }

                //perform action
                action.performAction();
                //lower currentM of user
                action.spendM();

                //!!!Here, we will do animations and text during the move
                actionCompleted = true;
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

            //update HUD
            setHUDs();

            if (actionCompleted)
            {
                //!!!later I want to have this reference numbers like damage dealt and stuff
                dialogueText.text = action.moveCompletedText();
                yield return new WaitForSeconds(2f);
            }

            //update Highlights
            setHighlights();
        }

        //clear the previous turn's actions
        clearSkills();

        //get rid of dead enemies (This only happens at the end of each turn since I want enemy revival to be possible)
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

    public void battleLost()
    {
        dialogueText.text = "you were defeated.";
    }

    public void setHUDs()
    {
        for (int i=0; i<team.Count; i++)
        {
            playerHUDs[i].setHUD(team[i]);
        }
    }

    public void setHighlights()
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

    public void clearSkills()
    {
        actionsToUse.Clear();
    }

    //used to sort actions in actionsToUse list
    public int compareActions(Action a, Action b)
    {
        if (a == null || b == null)
        {
            return 0;
        }

        int aUserSpeed = a.getUserSpeed();
        int bUserSpeed = b.getUserSpeed();

        return aUserSpeed.CompareTo(bUserSpeed);
    }

    public Unit findBestTarget(Unit target)
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
}
