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

    public GameObject playerPrefab;//will be array
    public GameObject enemyPrefab;//will be array

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    public int numTeam;
    public int numEnemies;
    public int numFighters;

    public List<Unit> team;
    public List<EnemyUnit> enemies;

    

    List<Action> actionsToUse = new List<Action>();//It would be nice if this works
    
    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;//will be array

    public MenuControl turnMenu;

    public Action attack;//!!!This will not be here

    // Start is called before the first frame update
    void Start()
    {
        //This is the first thing that has control in battles
        //It gets control in START ENEMYSELECT ATTACK WON and LOST battle states
        //The only thing the player can do when this has control is (sometimes) speedup/skip text or press a key to end the battle in WON and LOST
        hasControl = true;

        numFighters = numTeam + numEnemies;

        state = BattleState.START;
        StartCoroutine(setupBattle());
    }

    IEnumerator setupBattle()
    {
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        team.Add(playerGO.GetComponent<Unit>());

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemies.Add(enemyGO.GetComponent<EnemyUnit>());

        dialogueText.text = "A wild " + enemies[0].unitName + " approaches";

        playerHUD.setHUD(team[0]);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERSELECT;
        playerSelect();
    }

    void playerSelect()
    {
        dialogueText.text = "Choose an action";
        //give control to TurnMenu
        ControlManager.instance.switchControl(turnMenu);
    }

    public void enemySelect()
    {
        state = BattleState.ENEMYSELECT;
        //select enemy's moves one by one
        //load them into global list of moves

        Debug.Log("enemy is selecting moves");


        Action enemyMove = ScriptableObject.CreateInstance<ActionAttack>();
        enemyMove = enemies[0].selectAction();

        //don't put moves with no targets into turn order
        if (!enemyMove.hasNoTarget())
        {
            addAction(enemyMove);
        }

        //change to BATTLE phase
        state = BattleState.BATTLE;
        StartCoroutine(battle());
    }

    IEnumerator battle()//!!!This will be completely different
    {        
        Debug.Log("The battle is happening!");       
        //sort actionsToUse by unit speed
        actionsToUse.Sort(compareActions);



        //call actions one at a time
        foreach (Action action in actionsToUse)
        {
            bool actionCompleted = false;
            //make checks for things like unit death.
            if (action.getUser().currentH > 0)
            {//the user is alive

                if (!new List<int> { 0, 1 }.Contains(action.getTargetType()))
                {//this is a move with potentially dead targets
                    if (action.getHitsAll())
                    {//We only need remove dead targets
                     //get rid of all targets which are deaad
                        List<int> unitsToDelete = new List<int>();
                        for (int i = 0; i < action.getTargets().Count; i++)
                        {
                            if (action.getTargets()[i].currentH <= 0)
                            {//this target is dead. No need to target them
                                unitsToDelete.Add(i);
                            }
                        }
                        //delete necessary units
                        for (int j = 0; j < unitsToDelete.Count; j++)
                        {//delete targets backwards to avoid problems of changing indeces
                            action.removeTarget(unitsToDelete[unitsToDelete.Count - (j + 1)]);
                        }
                    }
                    else
                    {//We need to look for a suitable replacement target if the target is dead
                        if (action.getTargets()[0].currentH <= 0)
                        {
                            //!!!Our target is dead. Find best replacement if there is one
                            //if not, just delete this target and move on
                            action.removeTarget(0);
                        }
                    }                    
                }
                //No targets for this move are dead. Now we can perform the move
                //!!!This does not currently check if targetting move has no target
                action.performAction();
                actionCompleted = true;
            }

            //update HUD
            playerHUD.setHUD(team[0]);

            //dialogueText may be chaned when action is performed. I'm not sure yet
            if (actionCompleted)//is not called if would-be-user was dead
            {
                dialogueText.text = action.moveCompletedText();
                yield return new WaitForSeconds(2f);
            }
        }

        //clear the previous turn's actions
        clearSkills();

        //we're done performing actions for this turn.
        //check if either side has won.
        if (team[0].currentH==0)
        {
            //player lost
            state = BattleState.LOST;
            battleLost();
        }
        else if (enemies[0].currentH==0)
        {
            //player won!
            //change state to WON
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

    public void addAction(Action action)
    {
        actionsToUse.Add(action);
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

    public Unit findBestTarget(Action action)
    {//if the target is dead, it finds the best alternative if there is one. 
        //!!!I haven't written this yet because it is pointless with only 1 enemy and 1 player.
        return action.getTargets()[0];
    }
}
