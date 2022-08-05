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

    public Button playerButtonPrefab;
    public Button enemyButtonPrefab;

    public List<Button> playerButtons;
    public List<Button> enemyButtons;

    public override void Start()
    {
        //spawn a button above every unit
        foreach (Transform transform in BattleSystem.instance.enemyBattleStations)
        {
            spawnEnemyButton(transform);
        }
        foreach (Transform transform in BattleSystem.instance.playerBattleStations)
        {
            spawnTeamButton(transform);
        }

        selectedButton = firstButton;
        canvas.SetActive(false);
    }

    public void spawnTeamButton(Transform transform)
    {//spawns a target button above the player unit
        Transform buttonTransform = transform;
        buttonTransform.position = new Vector2(transform.position.x, transform.position.y + 10);//This number will be unique to the unit later on to accomadate differently sized enemies            
        Button button = Instantiate(playerButtonPrefab, buttonTransform);
    }

    public void spawnEnemyButton(Transform transform)
    {//spawns target button above the enemy unit, unit
        Transform buttonTransform = transform; 
        buttonTransform.position = new Vector2(transform.position.x, transform.position.y + 10);
        Button button = Instantiate(enemyButtonPrefab, buttonTransform);
    }

}
