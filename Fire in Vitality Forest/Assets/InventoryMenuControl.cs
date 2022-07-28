using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryMenuControl : Controllable
{
    public GameObject canvas;
    public Button firstButton;
    public Button selectedButton;

    // Start is called before the first frame update
    void Start()
    {
        selectedButton = firstButton;
        menuDepth = 2;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                ControlManager.instance.switchControl(OverworldMenuControl.instance);
            }
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
        }
        else
        {
            firstButton.Select();
            selectedButton = firstButton;
        }
    }
}
