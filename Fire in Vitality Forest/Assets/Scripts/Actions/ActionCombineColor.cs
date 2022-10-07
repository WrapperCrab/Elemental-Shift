using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CombineColor", menuName = "Actions/CombineColor")]
public class ActionCombineColor : Action
{
    public Element color;

    public override void performAction()
    {//effects on units in battle due to this move
        targets[0].combineColor(color);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " changed color!");
    }
}
