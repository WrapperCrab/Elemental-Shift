using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetButton : MonoBehaviour
{
    public Unit unit;
    public Action action;
    public void setButton(Unit _unit, Action _action)
    {
        unit = _unit;
        action = _action;
    }

    public void targetButtonPress()
    {
        //set action with the unit of the clicked button
        List<Unit> targets = new List<Unit>();
        targets.Add(unit);


        //var _action = Instantiate(action);
        action.setTargets(targets);

        //add the move to our list
        BattleSystem.instance.addAction(action);

        //!!!if all player's actions have been selected, change state to ENEMYSELECT
        ControlManager.instance.switchControl(BattleSystem.instance);
        BattleSystem.instance.enemySelect();
    }
}
