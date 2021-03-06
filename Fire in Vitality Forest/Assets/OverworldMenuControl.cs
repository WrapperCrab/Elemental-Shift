using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldMenuControl : Controllable
{
    #region Singleton

    public static OverworldMenuControl instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of OverworldMenuControl found!");
            return;
        }
        instance = this;
    }

    #endregion

    public GameObject canvas;
    public Button firstButton;

    // Start is called before the first frame update
    void Start()
    {
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.Escape)){
                switchMenu(0);//go back to player control
            }
        }
    }

    public void switchMenu(int buttonNumber)//!!!should only be activated when have control and did not switch control this frame
    { 
        switch (buttonNumber)
        {
            case 1:
                //ControlManager.instance.switchControl();
                break;
            case 2:
                //ControlManager.instance.switchControl();
                break;
            case 3:
                //ControlManager.instance.switchControl();
                break;
            case 4:
                //ControlManager.instance.switchControl();
                break;
            case 5:
                //ControlManager.instance.switchControl();
                break;
            case 6://quit
                ControlManager.instance.switchControl(QuitMenuControl.instance);
                break;
            default:
                ControlManager.instance.switchControl(PlayerMovement.instance);
                break;
        } 
    }

    public override void switchControl()
    {
        canvas.SetActive(!canvas.activeSelf);//!!!I don't want canvas to disappear unless I am going back to player. It works for now
        hasControl = !hasControl;
        if (canvas.activeSelf)
        {
            firstButton.Select();
        }
    }
}
