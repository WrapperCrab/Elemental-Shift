using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Actions/Attack")]
public class ActionAttack : Action
{
    public override void performAction()//this was previously stored in SkillList
    {//effects on units in battle due to this move
        int damage = BattleSystem.instance.getBaseDamage(user, targets[0], color);//damage formula is in this function
        targets[0].takeDamage(damage);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " took damage!");
    }
}
