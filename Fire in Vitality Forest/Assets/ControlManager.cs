using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlManager : MonoBehaviour
{
    #region Singleton

    public static ControlManager instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ControlManager found!");
            return;
        }
        instance = this;
    }

    #endregion


    //this is to be used to keep player control in only one place at a time. 
    //If a player switches to a different area of control in a frame, we have a bool here
    //which keeps the player's input from being counted again in the new area

    public bool switchedThisFrame = false;
    public Controllable obWithControl;

    void Start()
    {
        switchedThisFrame = false;
        obWithControl = PlayerMovement.instance;//!!!
    }

    void LateUpdate()
    {
        switchedThisFrame = false;//made false for the next frame
    }

    public bool getSwitched()
    {
        return switchedThisFrame;
    }

    public void switchControl(Controllable gainControl)
    {
        obWithControl.switchControl();
        gainControl.switchControl();
        obWithControl = gainControl;
        switchedThisFrame = true;
    }
}
