using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


public class TargetButton : MonoBehaviour, ISelectHandler, IDeselectHandler
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

    public void OnSelect(BaseEventData eventData)
    {
        //highlight unit
        unit.highlight();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //unhighlight unit
        unit.unHighlight();
    }
}
