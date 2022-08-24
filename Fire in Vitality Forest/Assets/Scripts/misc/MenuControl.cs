using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuControl : Controllable, IDeselectHandler
{
    //These things are alike to pretty much all menus, so this class is a no-brainer
    public GameObject canvas;
    public Button firstButton;
    public Button selectedButton;
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

        //make sure a button is ALWAYS selected
        staySelected();
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

    public virtual void OnDeselect(BaseEventData eventData)
    {
        Debug.Log("click outside menu");
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

    public void setCanvasCamera(Camera _camera)
    {
        canvas.GetComponent<Canvas>().worldCamera = _camera;
    }

    public void selectButton()
    {
        if (selectedButton != null)
        {
            selectedButton.Select();
        }
        else
        {
            selectedButton = firstButton;
            selectedButton.Select();
        }
    }

    public void staySelected()
    {//makes sure something on this menu is always selected

        //!!!It looks like this is working. As long as EventSystem.current.curr... is within the same menu (as it should be), then we should be golden

        if (hasControl)
        {
            if (EventSystem.current.currentSelectedGameObject != null)
            {
                selectedButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();//!!!
            }
            selectButton();
        }
    }
}
