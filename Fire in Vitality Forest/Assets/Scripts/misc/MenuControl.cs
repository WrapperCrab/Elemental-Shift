using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuControl : Controllable
{
    //These things are alike to pretty much all menus, so this class is a no-brainer
    public GameObject canvas;
    public Button firstButton;
    protected Button selectedButton;
    protected Controllable backMenu;//not menuControl since some menus go back to nonmenus

    // Start is called before the first frame update
    public virtual void Start()
    {
        selectedButton = firstButton;
        canvas.SetActive(false);
        //menuDepth assignment made in menu-specific scripts or in editor
    }

    public virtual void Update()
    {
        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pressedEscape();
            }
        }
    }

    public virtual void pressedEscape()//Called in Update when player presses escape
    {
        //update selectedButton
        if (EventSystem.current.currentSelectedGameObject != null)
        {
            selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();
        }
        //go to backMenu
        ControlManager.instance.switchControl(backMenu);
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            selectButton();
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
