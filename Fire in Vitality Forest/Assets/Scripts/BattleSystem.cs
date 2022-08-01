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

    public GameObject playerPrefab;
    public GameObject enemyPrefab;

    public Transform playerBattleStation;
    public Transform enemyBattleStation;

    Unit playerUnit;
    Unit enemyUnit;

    public TextMeshProUGUI dialogueText;

    public BattleHUD playerHUD;

    public MenuControl turnMenu;

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
        GameObject playerGO = Instantiate(playerPrefab, playerBattleStation);
        playerUnit = playerGO.GetComponent<Unit>();

        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleStation);
        enemyUnit = enemyGO.GetComponent<Unit>();

        dialogueText.text = "A wild " + enemyUnit.unitName + " approaches";

        playerHUD.setHUD(playerUnit);

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

        //change to BATTLE phase
        state = BattleState.BATTLE;
        StartCoroutine(battle());
    }

    IEnumerator battle()//!!!This will be completely different
    {
        //perform previously chosen actions one by one
        //wait a few seconds after each action so player can react

        Debug.Log("The battle is happening!");

        bool isEnemyDead = enemyUnit.getAttacked(playerUnit.attack);
        dialogueText.text = "the enemy took damage!";

        yield return new WaitForSeconds(2f);

        //!!!This is not how it will work
        if (isEnemyDead)
        {
            //change state to WON
            state = BattleState.WON;
            battleWon();
        }
        else
        {
            //do enemy's action
            bool isPlayerDead = playerUnit.getAttacked(enemyUnit.attack);

            playerHUD.setHUD(playerUnit);

            dialogueText.text = "you took damage!";

            yield return new WaitForSeconds(2f);

            if (isPlayerDead)
            {
                //change state to LOST
                state = BattleState.LOST;
                battleLost();
            }
            else
            {
                //change state to PLAYERSELECT
                state = BattleState.PLAYERSELECT;
                playerSelect();
            }
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
}
