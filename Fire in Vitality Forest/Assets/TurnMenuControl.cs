using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TurnMenuControl : Controllable
{

    public GameObject canvas;
    public Button firstButton;
    public Button selectedButton;


    // Start is called before the first frame update
    void Start()
    {
        selectedButton = firstButton;
        canvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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
            firstButton.Select();
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

    public void attackButtonPress()
    {

    }

    public void analyzeButtonPress()
    {

    }
}
