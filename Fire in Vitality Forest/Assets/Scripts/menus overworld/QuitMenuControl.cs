using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QuitMenuControl : Controllable
{
    public GameObject canvas;
    public Button firstButton;

    // Start is called before the first frame update
    void Start()
    {
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

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            firstButton.Select();
        }
    }

    public void noButtonPress()
    {
        ControlManager.instance.switchControl(OverworldMenuControl.instance);
    }

    public void yesButtonPress()
    {
        //!!!change scene to main menu
    }
}
