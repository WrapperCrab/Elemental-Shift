using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy1 : EnemyUnit
{
    
    //skills = {attack}

    public override Action selectAction()
    {
        //select move
        Action action = ScriptableObject.CreateInstance<ActionAttack>();
        action.setAction(skills[0]);

        //select user
        action.setUser(gameObject.GetComponent<EnemyUnit>());

        //select targets
        List<Unit> targets = new List<Unit>();
        targets.Add(BattleSystem.instance.team[0]);
        action.setTargets(targets);


        return action;
    }
}
