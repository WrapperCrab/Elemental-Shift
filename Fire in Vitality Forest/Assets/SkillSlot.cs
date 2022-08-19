using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class SkillSlot : MonoBehaviour, ISelectHandler
{
    public TextMeshProUGUI actionName;
    public TextMeshProUGUI mCost;

    //public TextMeshProUGUI selectedActionName;
    //public TextMeshProUGUI selectedActionDescription;

    Action action;//houses name, M cost, and description data

    public void setAction (Action _action)
    {
        action = _action;
        actionName.text = action.name;
        mCost.text = action.getMCost().ToString();

        gameObject.GetComponent<Button>().interactable = true;
    }

    public Action getAction()
    {
        return action;
    }

    public void clearAction()
    {
        action = null;
        actionName.text = "";
        mCost.text = "";

        gameObject.GetComponent<Button>().interactable = false;
    }

    public void enableButton()
    {
        gameObject.GetComponent<Button>().interactable = true;
    }

    public void OnSelect(BaseEventData eventData)
    {
        //change description and skill name in Skills Menu
        GetComponentInParent<SkillsMenuControl>().updateSelectedSkill(action);
    }
}
