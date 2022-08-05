using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public virtual Action selectAction()
    {//used by enemies to select their move
        //select move
        Action action = ScriptableObject.CreateInstance<ActionAttack>();
        action.setAction(normalAttack);

        //select user
        action.setUser(BattleSystem.instance.enemies[0]);

        //select targets
        List<Unit> targets = new List<Unit>();
        targets.Add(BattleSystem.instance.team[0]);
        action.setTargets(targets);


        return action;
    }
}
