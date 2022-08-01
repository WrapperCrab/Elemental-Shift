using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnMenuControl : MenuControl
{
    public MenuControl ActionSelectMenu;

    public override void Update()
    {
        //nothing needs to be done at this time
    }

    public override void changeAble()
    {
        //Kind of a bad solution, but this menu never needs to be disabled without being inactive.
        //So, when changeAble is called in controlSwitch, we simply want to call changeActive instead
        changeActive();
    }

    //BUTTON METHODS
    public void attackButtonPress()
    {
        //give control to attack menu
        //!!!Later we will need to do something to allow use of menu multiple times for each character
        ControlManager.instance.switchControl(ActionSelectMenu);
    }

    public void analyzeButtonPress()
    {

    }
}
