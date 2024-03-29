using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectMenuControl : MenuControl
{
    public PlayerUnit currentPlayer;//player having move selected right now
    public TargetSelectMenuControl targetSelectMenuPrefab;
    public SkillsMenuControl skillsMenuPrefab;
    public GameObject Panel;
    

    public void setActionSelectMenu(Controllable _backMenu, PlayerUnit _currentPlayer)
    {
        backMenu = _backMenu;
        currentPlayer = _currentPlayer;
    }

    public override void Start()
    {
        //spawn panel near current player
        Vector2 menuPosition = currentPlayer.GetComponent<Transform>().position;
        menuPosition.x += 1f;
        menuPosition.y += 1f;
        setPosition(menuPosition);

        selectedButton = firstButton;
    }

    public override void pressedEscape()//Called in Update when player presses escape
    {
        //go to backMenu
        ControlManager.instance.switchControl(backMenu);

        //if action list is non-empty, remove most recently decided action
        BattleSystem.instance.removeAction();

        //remove this menu in TurnMenuControl
        TurnMenuControl.instance.unselectAction();

        //destroy this menu
        Destroy(gameObject);

    }

    public override void changeAble()
    {
        changeActive();
    }

    public void setPosition(Vector2 position)
    {
        Panel.GetComponent<Transform>().position = position;
    }

    public void playerAction(Action action)//called when a skill button is pressed
    {
        //create copy of action
        var _action = Instantiate(action);//!!!Creates an independent clone of action... I think

        //set user
        _action.setUser(currentPlayer);

        //send it to TargetSelectMenu to set targets
        TargetSelectMenuControl targetSelectMenu = Instantiate(targetSelectMenuPrefab, gameObject.GetComponent<Transform>());
        targetSelectMenu.setAction(_action);
        targetSelectMenu.canvas.SetActive(false);
        targetSelectMenu.setCanvasCamera(canvas.GetComponent<Canvas>().worldCamera);
        targetSelectMenu.setBackMenu(gameObject.GetComponent<ActionSelectMenuControl>());
        ControlManager.instance.switchControl(targetSelectMenu);
    }

    public void playerPass()
    {
        //change to next player
        TurnMenuControl.instance.nextPlayer();

        //update the nextBackMenu
        //!!!This is not working
        TurnMenuControl.instance.setNextBackMenu(gameObject.GetComponent<ActionSelectMenuControl>());

        //give control to next menu
        ControlManager.instance.switchControl(TurnMenuControl.instance);
    }

    public void createSkillsMenu()
    {
        //instantiate the skillsMenu prefab
        SkillsMenuControl skillsMenu = Instantiate(skillsMenuPrefab, gameObject.GetComponent<Transform>());//!!!This is now where I will want this to spawn

        skillsMenu.setSkillsMenu(currentPlayer, gameObject.GetComponent<ActionSelectMenuControl>());
        skillsMenu.canvas.SetActive(false);
        skillsMenu.setCanvasCamera(canvas.GetComponent<Canvas>().worldCamera);

        //switch control to the skillsMenu
        ControlManager.instance.switchControl(skillsMenu);
    }
}
