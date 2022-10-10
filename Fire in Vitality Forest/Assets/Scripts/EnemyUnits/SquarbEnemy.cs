using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquarbEnemy : EnemyUnit
{

    //skills = {combineRed, combineYellow}

    public override Action selectAction()
    {
        //ActionCombineColor action;

        var action = ScriptableObject.CreateInstance<ActionCombineColor>();

        //select user
        Unit user = gameObject.GetComponent<EnemyUnit>();

        //select targets
        Unit target = BattleSystem.instance.team[0];//!!!Doesn't check if dead
        Debug.Log(target.unitName);

        //select and set move
        Element tColor = target.getColor();
        switch (tColor)
        {
            case Element.R:
            case Element.B:
            case Element.G:
            default:
                //action.setAction(user.skills[0]);//combineRed
                action.setColor(Element.R);
                break;
            case Element.c:
            case Element.m:
            case Element.y:
            case Element.k:
                action.setAction(user.skills[1]);//combineYellow
                action.setColor(Element.y);
                break;
        }

        //set user
        action.setUser(user);

        //set target
        action.addTarget(target);

        return action;
    }
}
