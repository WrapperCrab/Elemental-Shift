using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    public List<GameObject> playerPrefabs;
    public List<GameObject> enemyPrefabs;
    public List<int> spawnLocations;//battlestation to spawn each enemy on

    public List<Transform> playerBattleStations;
    public List<Transform> enemyBattleStations;

    public List<PlayerUnit> team;
    public List<EnemyUnit> enemies;

    public List<Action> actionsToUse = new List<Action>();//It would be nice if this works
    
    public TextMeshProUGUI dialogueText;

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
        //spawn the players
        for (int i=0; i<playerPrefabs.Count; i++)
        {
            GameObject playerGO = Instantiate(playerPrefabs[i], playerBattleStations[i]);
            team.Add(playerGO.GetComponent<PlayerUnit>());
        }

        //spawn the enemies
        for (int i = 0; i < enemyPrefabs.Count; i++)
        {
            GameObject enemyGO = enemyBattleStations[spawnLocations[i]].GetComponent<EnemyBattleStation>().fillStation(enemyPrefabs[i]);
            enemies.Add(enemyGO.GetComponent<EnemyUnit>());
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
            //don't put moves with no targets into turn order
            //!!!Later on there may be moves with no targets that do do something. For now though, they are all treated as passes
            if (!enemyMove.hasNoTarget())
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
                        default;
                            //we are ready to do the move
                            break;
                        
                        case 2://1 teammate
                        case 4://1 enemy
                        case 6://1 unit
                            //check if target is dead
                            allTargetsDead = action.getAllTargetsDead();
                            if (allTargetsDead)
                            {//find a replacement
                                Unit oldTarget = action.getTargets[0];
                                Unit replacement = findBestTarget(oldTarget);
                                if (replacement != null)
                                {//we found one! replace it in action
                                    action.removeTarget(oldTarget);
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
                        actionCompleted = true;
                    }
                    else if (userDead)
                    {

                    }else if (insufficientM)
                    {

                    }else if (allTargetsDead)
                    {

                    }
                    else
                    {

                    }
                }
            }



            //if (action.getUser().currentH > 0)
            //{//the user is alive


            //    //check for too low magic if player
            //    PlayerUnit convertedUser = action.getUser() as PlayerUnit;
            //    bool userIsPlayer = (convertedUser != null);
            //    if (!userIsPlayer || (convertedUser.currentM >= action.getMCost()))//!!!must only make check if user is PlayerUnit
            //    {//the user has enough M or is an enemy
            //        if (!new List<int> { 0, 1 }.Contains(action.getTargetType()))
            //        {//this is a move with potentially dead targets
            //            if (action.getHitsAll())
            //            {//We only need remove dead targets
            //                bool targetsLeft = action.removeAllDeadTargets();
            //                if (!targetsLeft)
            //                {
            //                    skipThisMove = true;
            //                }
            //            }
            //            else
            //            {//We need to look for a suitable replacement target if the target is dead
            //                if (action.getTargets()[0].currentH <= 0)
            //                {//our target is dead. Find a replacement
            //                 //remove the old target
            //                    Unit target = action.getTargets()[0];
            //                    action.removeTarget(target);

            //                    //find original target type, then find best replacement
            //                    Unit replacement = findBestTarget(target);

            //                    if (replacement == null)
            //                    {//if no replacement, skip this action
            //                        skipThisMove = true;
            //                    }
            //                    else
            //                    {//add this as the new target
            //                        action.addTarget(replacement);
            //                    }
            //                }
            //            }
            //        }

            //        if (!skipThisMove)
            //        {//we are good to go with using this move
            //         //set highlights for this action
            //            action.getUser().setHighlight(Highlight.ACTING);
            //            foreach (Unit target in action.getTargets())
            //            {
            //                target.setHighlight(Highlight.TARGETTED);
            //            }

            //            //perform action
            //            action.performAction();
            //            actionCompleted = true;
            //        }
            //    }//else//the user is a player with not enough M
            //}

            //update HUD
            setHUDs();

            //dialogueText may be changed when action is performed. I'm not sure yet
            if (actionCompleted)//is not called if would-be-user was dead
            {
                dialogueText.text = action.moveCompletedText();
                yield return new WaitForSeconds(2f);
            }
            //!!! I want different messages depending on why the move failed

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



        //we're done performing actions for this turn.
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
            battleWon();
        }
        else
        {
            //change state to PLAYERSELECT
            state = BattleState.PLAYERSELECT;
            playerSelect();
        }
    }

    public void battleWon()
    {
        dialogueText.text = "you won the battle!";
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

    public void clearSkills()//!!!I don't think this works yet
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
