using System.Collections.Generic;
using System;
using UnityEngine;

[CreateAssetMenu(fileName = "CombineColor", menuName = "Actions/CombineColor")]
public class ActionCombineColor : Action
{
    public override void performAction()
    {//effects on units in battle due to this move

        base.performAction();//call "attack" animation for user if needed
        targets[0].combineColor(color);
    }

    public override string moveCompletedText()
    {//text displayed after this move was used
        return (targets[0].unitName + " changed color!");
    }

    public override void updateColor()
    {
        //do nothing. Color is set be setColor, not user
    }

    public void setColor(Element newColor)
    {
        color = newColor;
    }
}
