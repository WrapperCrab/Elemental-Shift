using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitMenuControl : MenuControl
{
    public void noButtonPress()
    {
        ControlManager.instance.switchControl(OverworldMenuControl.instance);
    }

    public void yesButtonPress()
    {
        //!!!change scene to main menu
    }
}
