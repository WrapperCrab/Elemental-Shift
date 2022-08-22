using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillSlot : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI actionNameText;
    public TextMeshProUGUI mCostText;

    Action action;//action name, M cost, and description data
    PlayerUnit user;

    public void setAction (Action _action)
    {
        action = _action;
        actionNameText.text = action.name;

        int mCost = action.getMCost();
        mCostText.text = mCost.ToString();

        if (user.currentM < mCost)
        {//This is too expensive right now. Must let player know that
            //make the button red
            GetComponent<Image>().color = Color.red;
        }

        gameObject.GetComponent<Button>().interactable = true;
    }

    public Action getAction()
    {
        return action;
    }

    public void clearAction()
    {
        action = null;
        actionNameText.text = "";
        mCostText.text = "";

        gameObject.GetComponent<Button>().interactable = false;
    }

    public void enableButton()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void setUser(PlayerUnit _user) 
    {
        user = _user;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //change description and skill name in Skills Menu
        GetComponentInParent<SkillsMenuControl>().updateSelectedSkill(action);
    }

    public void OnClick()//Note! this was assigned to the button by me.
    {
        //call function in SkillsMenuControl
        GetComponentInParent<SkillsMenuControl>().playerAction(action);
    }
}
