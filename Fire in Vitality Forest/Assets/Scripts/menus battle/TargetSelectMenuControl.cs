using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TargetSelectMenuControl : MenuControl
{
    //here we need to display the correct buttons based on the action selected in ActionSelectmenu
    //Then we need to allow player to press one of these buttons
    //Then we set the targets
    //then we add the full action to actionsToUse
    //Then we switch to the actionSelectMenu for the next team member if there is one
    //and we must do this in a way that allows the player to go back and reselect moves

    public Action action;

    public Button targetButtonPrefab;
    public Button confirmationButton;
    public bool buttonsCreated = false;

    public List<Button> targetButtons;

    public override void Start()//want to pass in current action to this for initialization
    {
        //decide based on action what to spawn
        bool useConfirmationScreen = false;

        int actionType = action.getTargetType();
        List<int> confirmationTypes = new List<int>{ 0, 1, 3, 5, 7 };
        if (confirmationTypes.Contains(actionType))
        {
            useConfirmationScreen = true;
        }

        if (useConfirmationScreen)
        {//spawn the confirmation screen and highlight targets
            //find which players are targetted
            List<Unit> targettedUnits = new List<Unit>();
            switch (actionType)
            {
                case 0:
                default:
                    break;
                case 1:
                    //targettedUnits.Add();//!!!Find current player
                    break;
                case 3:
                    targettedUnits.AddRange(BattleSystem.instance.team);
                    break;
                case 5:
                    targettedUnits.AddRange(BattleSystem.instance.enemies);
                    break;
                case 7:
                    targettedUnits.AddRange(BattleSystem.instance.team);
                    targettedUnits.AddRange(BattleSystem.instance.enemies);
                    break;
            }
            //Highlight targettedplayers
            foreach (Unit unit in targettedUnits)
            {
                highlightUnit(unit);
            }
            //spawn and set the confirmation button
            Button button = Instantiate(confirmationButton, canvas.transform);
            targetButtons.AddRange(gameObject.GetComponentsInChildren<Button>());
            firstButton = targetButtons[0];

            button.GetComponent<TargetConfirmButton>().setButton(targettedUnits, action);
        }
        else
        {//spawn buttons above viable targets' heads. Highlight selected target
            //!!!I need to check for viability

            List<Unit> viableTargets = new List<Unit>();
            bool onEnemy = action.onEnemy;
            bool onTeam = action.onTeam;
            if (onEnemy)
            {//add enemies to list of viable targets
                viableTargets.AddRange(BattleSystem.instance.enemies);
            }
            if (onTeam)
            {//add team to list of viable targets
                viableTargets.AddRange(BattleSystem.instance.team);
            }

            foreach (Unit unit in viableTargets)
            {
                //create button above unit
                spawnTargetButton(unit.GetComponent<Transform>().position, unit);
            }
            //assign firstButton
            targetButtons.AddRange(gameObject.GetComponentsInChildren<Button>());
            firstButton = targetButtons[0];
        }
        buttonsCreated = true;
        selectButton();
    }

    public void setAction(Action _action)
    {
        action = Instantiate(_action);
    }
    public void setBackMenu(ActionSelectMenuControl _backMenu)
    {
        backMenu = _backMenu;
    }
    public void setCanvasCamera(Camera _camera)
    {
        canvas.GetComponent<Canvas>().worldCamera = _camera;
    }

    public void spawnTargetButton(Vector3 unitPosition, Unit unit)
    {
        ////spawn a target button above the unit
        Vector3 buttonPosition = unitPosition;
        buttonPosition.y += 2f;//in the future, this may be unique to the unit
        Button button = Instantiate(targetButtonPrefab, buttonPosition, Quaternion.identity, canvas.transform);

        //change button text
        button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text = unit.name;

        //change button's held members
        button.GetComponent<TargetButton>().setButton(unit, action);
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            if (buttonsCreated)
            {
                selectButton();
            }
        }
        else
        {
            //unhighlight all units
            foreach (Unit unit in BattleSystem.instance.team)
            {
                unHighlightUnit(unit);
            }
            foreach (Unit unit in BattleSystem.instance.enemies)
            {
                unHighlightUnit(unit);
            }
            //delete this menu
            Destroy(gameObject);
        }
    }

    public override void changeAble()
    {
        //there is no cursor memory on this menu. Like in persona Q, you cannot go back to this menu, you can only go forward into it
        changeActive();
    }

    public void highlightUnit(Unit unit)
    {
        unit.highlight();
    }

    public void unHighlightUnit(Unit unit)
    {
        unit.unHighlight();
    }
}
