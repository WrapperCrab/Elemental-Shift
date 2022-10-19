using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyEnemy : EnemyUnit
{

    //skills = {attack, absorb}

    public override Action selectAction()
    {
        Action action;
        //select move
        if (currentH > 5)
        {//use attack
            action = ScriptableObject.CreateInstance<ActionAttack>();
            action.setAction(skills[0]);
        }
        else
        {//use absorb
            action = ScriptableObject.CreateInstance<ActionAbsorb>();
            action.setAction(skills[1]);
        }

        //select user
        action.setUser(gameObject.GetComponent<EnemyUnit>());

        //select targets
        List<Unit> targets = new List<Unit>();
        targets.Add(BattleSystem.instance.team[0]);
        action.setTargets(targets);


        return action;
    }
}
