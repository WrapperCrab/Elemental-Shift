using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;

public class orderButton : MonoBehaviour, ISelectHandler, IDeselectHandler
{//this button does nothing when pressed
    Unit unit;
    AnalyzeMenuControl menu;
    public TextMeshProUGUI playerNumText;

    public void setButton(Unit _unit, int _playerNum, AnalyzeMenuControl _menu)
    {
        unit = _unit;
        menu = _menu;
        playerNumText.text = _playerNum.ToString();
    }

    public void OnSelect(BaseEventData eventData)
    {
        //spawn info panel for this unit
        menu.spawnInfoPanel(unit);

        //highlight this unit
        unit.setHighlight(Highlight.TARGETTED);
    }

    public void OnDeselect(BaseEventData eventData)
    {
        //unhighlight this unit
        unit.setHighlight(Highlight.NONE);
    }

}
