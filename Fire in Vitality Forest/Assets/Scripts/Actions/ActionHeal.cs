using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Actions/Heal")]
public class ActionHeal : Action
{
    public override void performAction()//this was previously stored in SkillList
    {//effects on units in battle due to this move
        int heal = 10;
        targets[0].gainHealth(heal);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " gained health!");
    }
}
