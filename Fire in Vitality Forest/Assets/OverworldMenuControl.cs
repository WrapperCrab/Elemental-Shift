using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
    public Button selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = firstButton;
        menuDepth = 1;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.E))
            {
                switchMenu(0);//go back to player control
            }
        }
    }

    public void switchMenu(int buttonNumber)//!!!should only be activated when have control and did not switch control this frame
    {
        //update selectedButton
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }
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

    public override void changeAble()
    {
        //will disable UI elements if applicable
        Button[] buttons = canvas.GetComponentsInChildren<Button>();
        bool canvasEnabled = !firstButton.interactable;
        foreach (Button b in buttons) 
        { 
            b.interactable = canvasEnabled;
        }
        if (canvasEnabled)
        {
            selectButton();
        }
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            selectButton();
        }
    }

    public void selectButton()
    {
        if (selectedButton != null)
        {
            selectedButton.Select();
            Debug.Log("selected button");
        }
        else
        {
            firstButton.Select();
            selectedButton = firstButton;
        }
    }
}
