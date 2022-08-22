using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetConfirmButton : MonoBehaviour
{
    public List<Unit> targets;
    public Action action;
    public void setButton(List<Unit> _targets, Action _action)
    {
        targets = _targets;
        action = _action;
    }

    public void confirmButtonPress()
    {
        //set action with the targets
        action.setTargets(targets);

        //add the move to our list
        BattleSystem.instance.addAction(action);

        //update TurnMenu
        TurnMenuControl.instance.nextPlayer();

        //set nextBackMenu in TurnMenu to targetSelect's backMenu
        TurnMenuControl.instance.setNextBackMenu(gameObject.GetComponentInParent<TargetSelectMenuControl>().getBackMenu());

        //give control back to TurnMenu
        ControlManager.instance.switchControl(TurnMenuControl.instance);


    }

}
