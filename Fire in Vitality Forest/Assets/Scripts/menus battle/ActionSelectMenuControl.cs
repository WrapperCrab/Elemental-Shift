using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectMenuControl : MenuControl
{
    public Skill attack;//every character has a basic attack, so I don't need to worry about this being in their skill list

    public void attackButtonPress()
    {
        playerAttack();
    }

    public void playerAttack()
    {
        //!!!This is not yet made for more than 1 player
        Action _attack = ScriptableObject.CreateInstance<Action>();

        //initialize targets. Not sure if this is the best way to do this
        Unit[] targets = new Unit[1];
        targets[0] = BattleSystem.instance.enemies[0];

        _attack.setAction(attack, BattleSystem.instance.team[0], targets);
        BattleSystem.instance.addAction(_attack);

        //change to next player's attack menu... somehow

        //if all player's actions have been selected, change state to ENEMYSELECT
        ControlManager.instance.switchControl(BattleSystem.instance);
        BattleSystem.instance.enemySelect();
    }
}
