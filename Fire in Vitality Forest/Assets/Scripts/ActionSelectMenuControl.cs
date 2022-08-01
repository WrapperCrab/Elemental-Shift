using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectMenuControl : MenuControl
{

    public void attackButtonPress()
    {
        playerAttack();
    }

    public void playerAttack()
    {
        //!!!save player's decision to some global list of actions to be performed durring the BATTLE phase
        //Not going to do this now, but that will be the next step after I get basic loop working

        //change to next player's attack menu... somehow

        //if all player's actions have been selected, change state to ENEMYSELECT
        ControlManager.instance.switchControl(BattleSystem.instance);
        BattleSystem.instance.enemySelect();
    }
}
