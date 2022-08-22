using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Absorb", menuName = "Actions/Absorb")]
public class ActionAbsorb : Action
{
    public override void performAction()//this was previously stored in SkillList
    {//effects on units in battle due to this move
        int attack = user.attack;
        int defense = targets[0].defense;
        int damage = (int)Math.Ceiling(((double)attack / (double)defense));

        targets[0].takeDamage(damage);
        user.takeDamage(-damage);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " took damage and " + user.unitName + " healed!");
    }
}
