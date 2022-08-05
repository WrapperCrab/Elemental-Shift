using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectMenuControl : MenuControl
{
    public PlayerUnit currentPlayer;//player having move selected right now
    public TargetSelectMenuControl targetSelectMenuPrefab;

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            selectButton();
            currentPlayer = BattleSystem.instance.team[0];
        }
    }

    public void playerAction(Action action)//called when a skill button is pressed
    {
        //create copy of action
        var _action = Instantiate(action);//!!!Creates an independent clone of action... I think

        //set user
        _action.setUser(currentPlayer);

        //send it to TargetSelectMenu to set targets
        TargetSelectMenuControl targetSelectMenu = Instantiate(targetSelectMenuPrefab);
        targetSelectMenu.setAction(action);
    }


    public void playerAttack()
    {
        //!!!This is not yet made for more than 1 player
        var _attack = Instantiate(currentPlayer.normalAttack);
        _attack.setUser(currentPlayer);

        //initialize targets. Not sure if this is the best way to do this
        List<Unit> targets = new List<Unit>();
        targets.Add(BattleSystem.instance.enemies[0]);

        _attack.setTargets(targets);
        BattleSystem.instance.addAction(_attack);

        //change to next player's attack menu... somehow

        //if all player's actions have been selected, change state to ENEMYSELECT
        ControlManager.instance.switchControl(BattleSystem.instance);
        BattleSystem.instance.enemySelect();
    }

    public void playerPass()
    {
        //change to next player's attack menu

        //if all player's actions have been selected, change state to ENEMYSELECT
        ControlManager.instance.switchControl(BattleSystem.instance);
        BattleSystem.instance.enemySelect();

    }
}
