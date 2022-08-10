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

    public Button playerButtonPrefab;
    public Button enemyButtonPrefab;
    public GameObject ConfirmationScreenPrefab;

    public List<Button> playerButtons;
    public List<Button> enemyButtons;

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
            Instantiate(ConfirmationScreenPrefab);//!!!

            //Highlight players in action.getTargets
            foreach (Unit unit in action.getTargets())
            {
                highlightUnit(unit);
            }
        }
        else
        {//spawn buttons above viable targets' heads. Highlight selected target
            //!!!I need to check for viability

            List<Unit> viableTargets = new List<Unit>();
            bool onEnemy = action.onEnemy;
            bool onTeam = action.onTeam;
            if (onEnemy)
            {//add enemies to list of viable targets
                viableTargets.AddRange(BattleSystem.instance.team);
            }
            if (onTeam)
            {//add team to list of viable targets
                viableTargets.AddRange(BattleSystem.instance.enemies);
            }

            foreach (Unit unit in viableTargets)
            {
                //highlight unit
                highlightUnit(unit);
                //!!!create button above unit
                spawnEnemyButton(unit.GetComponent<Transform>());
            }
        }

        selectedButton = firstButton;
    }

    public void setAction(Action _action)
    {
        action = Instantiate(_action);
    }

    public void spawnTeamButton(Transform transform)
    {//spawns a target button above the player unit
        var buttonTransform = Instantiate(transform);
        buttonTransform.position = new Vector2(transform.position.x, transform.position.y);//This number will be unique to the unit later on to accomadate differently sized enemies            
        Button button = Instantiate(playerButtonPrefab, buttonTransform.position, Quaternion.identity, canvas.transform);
    }

    public void spawnEnemyButton(Transform transform)
    {//spawns target button above the enemy unit, unit
        Transform buttonTransform = Instantiate(transform); 
        buttonTransform.position = new Vector2(transform.position.x, transform.position.y);
        Button button = Instantiate(enemyButtonPrefab, buttonTransform.position, Quaternion.identity, canvas.transform);
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            selectButton();
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
        //I may not want to do this if I want to have cursor memory for this menu. For now though, it will do
        changeActive();

        //Button[] buttons = canvas.GetComponentsInChildren<Button>();
        //bool canvasEnabled = !firstButton.interactable;
        //foreach (Button b in buttons)
        //{
        //    b.interactable = canvasEnabled;
        //}
        //if (canvasEnabled)
        //{
        //    selectButton();
        //}
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
