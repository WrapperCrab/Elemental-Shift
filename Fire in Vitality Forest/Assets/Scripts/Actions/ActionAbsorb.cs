using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Absorb", menuName = "Actions/Absorb")]
public class ActionAbsorb : Action
{
    public override void performAction()
    {//effects on units in battle due to this move
        int damage = BattleSystem.instance.getBaseDamage(user, targets[0], color);

        targets[0].takeDamage(damage);
        user.gainHealth(damage);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " took damage and " + user.unitName + " healed!");
    }
}
