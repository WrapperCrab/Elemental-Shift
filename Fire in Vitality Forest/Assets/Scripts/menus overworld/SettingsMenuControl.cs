using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuControl : Controllable
{
    public GameObject canvas;
    public Button firstButton;//!!!Not sure if this will work for sliders

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

    public void setArbNum(float arbNum)
    {
        Debug.Log(arbNum);
    }
}
