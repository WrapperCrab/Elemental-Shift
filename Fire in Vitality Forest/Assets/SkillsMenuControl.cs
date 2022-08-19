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
    }

    public override void Start()
    {
        slots = gameObject.GetComponentsInChildren<SkillSlot>();
        foreach (SkillSlot button in slots)
        {
            button.GetComponent<Button>().interactable = false;
        }

        //set a skill for each button
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < currentPlayer.skills.Count)
            {
                slots[i].setAction(currentPlayer.skills[i]);
            }
            else
            {
                break;
            }
        }

        firstButton = slots[0].GetComponent<Button>();//!!!
        selectedButton = firstButton;
        staySelected();
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
        staySelected();
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
    }
}
