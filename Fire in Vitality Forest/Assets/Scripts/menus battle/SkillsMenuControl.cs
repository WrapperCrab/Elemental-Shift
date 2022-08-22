using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillsMenuControl : MenuControl
{
    public PlayerUnit currentPlayer;//player having move selected right now
    public TargetSelectMenuControl targetSelectMenuPrefab;

    public TextMeshProUGUI title;
    SkillSlot[] slots;
    public TextMeshProUGUI skillName;
    public TextMeshProUGUI skillDescription;
    public TextMeshProUGUI warning;
    public TextMeshProUGUI MText;

    bool firstButtonFound = false;

    public void setSkillsMenu(PlayerUnit _currentPlayer, MenuControl _backMenu)
    {
        currentPlayer = _currentPlayer;
        backMenu = _backMenu;
    }
    public void setCanvasCamera(Camera _camera)
    {
        canvas.GetComponent<Canvas>().worldCamera = _camera;
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            Start2();
        }
    }

    public override void Start()
    {

    }

    public void Start2()
    {
        title.text = currentPlayer.unitName + "'s Skills";
        MText.text = currentPlayer.currentM.ToString();

        slots = gameObject.GetComponentsInChildren<SkillSlot>();
        foreach (SkillSlot button in slots)
        {
            button.GetComponent<Button>().interactable = false;
            button.setUser(currentPlayer);
        }

        //set a skill for each button
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < currentPlayer.skills.Count)
            {
                slots[i].setAction(currentPlayer.skills[i]);
                if (!firstButtonFound)
                {
                    firstButton = slots[i].GetComponent<Button>();
                    firstButtonFound = true;
                }
            }
            else
            {
                break;
            }
        }

        if (firstButtonFound)
        {
            selectButton();
        }//otherwise, there is no button to select which is okay
    }


    public override void Update()
    {
        if (hasControl && !ControlManager.instance.getSwitched())
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                pressedEscape();
            }
        }

        //!!!we may not want to do this for this menu since there may be no buttons
        //make sure a button is ALWAYS selected
        if (firstButtonFound)
        {
            staySelected();
        }
    }

    public override void pressedEscape()//Called in Update when player presses escape
    {
        //go to backMenu
        ControlManager.instance.switchControl(backMenu);

        //destroy this menu
        Destroy(gameObject);
    }

    public void updateSelectedSkill(Action action)
    {
        skillName.text = action.name;
        skillDescription.text = action.description;
        if (currentPlayer.currentM < action.getMCost())
        {//they don't have enough M. provide a warning
            warning.text = "You don't currently have enough M to use that action";
        }
        else
        {
            warning.text = "";
        }
    }

    public void playerAction(Action action)//called when a skill button is pressed
    {
        //create copy of action
        var _action = Instantiate(action);//!!!Creates an independent clone of action... I think

        //set user
        _action.setUser(currentPlayer);

        //send it to TargetSelectMenu to set targets
        TargetSelectMenuControl targetSelectMenu = Instantiate(targetSelectMenuPrefab, gameObject.GetComponent<Transform>());
        targetSelectMenu.setAction(_action);
        targetSelectMenu.canvas.SetActive(false);
        targetSelectMenu.setCanvasCamera(canvas.GetComponent<Canvas>().worldCamera);
        targetSelectMenu.setBackMenu(gameObject.GetComponent<SkillsMenuControl>());
        ControlManager.instance.switchControl(targetSelectMenu);
    }
}
