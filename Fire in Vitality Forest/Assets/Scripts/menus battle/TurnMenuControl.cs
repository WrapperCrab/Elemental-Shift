using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnMenuControl : MenuControl
{
    #region Singleton

    public static TurnMenuControl instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of TurnMenuControl found!");
            return;
        }
        instance = this;
    }

    #endregion

    int playerNum = 0;//increases each time an action is added to the list
    List<PlayerUnit> actionablePlayers = new List<PlayerUnit>();//players which are able to perform actions this turn
    bool goToNextPhase = false;
    bool goToNextPlayer = false;

    public ActionSelectMenuControl actionSelectMenuPrefab;
    List<ActionSelectMenuControl> actionSelectMenus = new List<ActionSelectMenuControl>();//list of all instantiated menus of this type

    #region menu stuff

    public override void Update()
    {
        if (goToNextPhase)
        {
            //destroy all actionSelect menus
            foreach (ActionSelectMenuControl action in actionSelectMenus)
            {
                Destroy(action.gameObject);
            }
            actionSelectMenus.Clear();

            //reset playerNum
            playerNum = 0;

            //reset actionablePlayers
            actionablePlayers.Clear();

            //switch to next phase
            ControlManager.instance.switchControl(BattleSystem.instance);
            BattleSystem.instance.enemySelect();
        }
        else if (goToNextPlayer)
        {
            attackButtonPress();
        }
        //nothing needs to be done otherwise
    }

    public void LateUpdate()
    {
        goToNextPhase = false;
        goToNextPlayer = false;
    }

    public override void changeActive()
    {
        if ((playerNum == actionablePlayers.Count) && (actionablePlayers.Count!=0))
        {//all players' actions have been chosen. We can move on to the ENEMYSELECT phase
            goToNextPhase = true;
        }
        else if (playerNum != 0)
        {//go to this player's action select menu
            goToNextPlayer = true;
        }
        else if (!goToNextPhase)
        {//playerNum==0 and we are not switching to the next phase; activate or deactivate this menu for real
            canvas.SetActive(!canvas.activeSelf);
            if (canvas.activeSelf)
            {
                selectButton();
                updateActionablePlayers();
            }
        }
    }

    public override void changeAble()
    {
        changeActive();
    }

    //BUTTON METHODS
    public void attackButtonPress()
    {//give control to attack menu
        //spawn actionSelectMenu for this player
        PlayerUnit currentPlayer = actionablePlayers[playerNum];
        ActionSelectMenuControl actionSelectMenu = Instantiate(actionSelectMenuPrefab, GetComponent<Transform>());
        int newIndex = actionSelectMenus.Count;
        actionSelectMenus.Add(actionSelectMenu);

        //set the variables for this new menu
        actionSelectMenu.canvas.SetActive(false);
        actionSelectMenu.setCanvasCamera(canvas.GetComponent<Canvas>().worldCamera);
        if (playerNum == 0)
        {
            actionSelectMenu.setActionSelectMenu(TurnMenuControl.instance, currentPlayer);
        }
        else
        {
            actionSelectMenu.setActionSelectMenu(actionSelectMenus[newIndex - 1], currentPlayer);
        }

        //switch control to this new menu
        ControlManager.instance.switchControl(actionSelectMenu);
    }

    public void analyzeButtonPress()
    {

    }

    #endregion

    public void nextPlayer()
    {
        playerNum++;
    }

    public void unselectAction()
    {//called when pressing escape on an actionSelectMenu
        if (playerNum != 0)
        {
            playerNum--;
            actionSelectMenus.RemoveAt(actionSelectMenus.Count - 1);
        }
        else
        {//playerNum==0
            actionSelectMenus.Clear();
        }
    }

    void updateActionablePlayers()
    {//get actionable players
        foreach (PlayerUnit player in BattleSystem.instance.team)
        {//check if unit is actionable
            if (player.currentH > 0)
            {
                actionablePlayers.Add(player);
            }
        }
    }


}
