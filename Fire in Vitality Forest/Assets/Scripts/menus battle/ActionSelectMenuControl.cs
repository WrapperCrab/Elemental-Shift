using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSelectMenuControl : MenuControl
{
    //every character has these skills, so I don't need to worry about this being in their skill list
    public Action attack;

    public void playerAttack()
    {
        //!!!This is not yet made for more than 1 player
        Action _attack = ScriptableObject.CreateInstance<ActionAttack>();
        _attack.setAction(attack);

        //initialize targets. Not sure if this is the best way to do this
        List<Unit> targets = new List<Unit>();
        targets.Add(BattleSystem.instance.enemies[0]);


        _attack.setUser(BattleSystem.instance.team[0]);
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
