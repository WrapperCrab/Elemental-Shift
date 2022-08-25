using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyzeMenuControl : MenuControl
{
    public Button orderButtonPrefab;//these buttons do nothing when clicked, but spawn panel when selected
    public infoPanel infoPanelPrefab;//displays unit info
    infoPanel infoPanelInstance;

    List<Button> orderButtons = new List<Button>();//for setting firstButton after all buttons are spawned
    List<Unit> units = new List<Unit>();

    bool buttonsCreated = false;

    public override void Start()
    {
        units.AddRange(BattleSystem.instance.enemies);
        units.AddRange(BattleSystem.instance.team);
        //sort the units by speed
        units.Sort(compareUnits);

        int playerNum = 1;
        foreach (Unit unit in units)
        {
            //create button above unit
            spawnOrderButton(unit.GetComponent<Transform>().position, unit, playerNum);
            //increase playerNum
            playerNum++;
        }
        //assign firstButton
        orderButtons.AddRange(gameObject.GetComponentsInChildren<Button>());
        firstButton = orderButtons[0];

        buttonsCreated = true;
        selectButton();
    }

    public override void changeActive()
    {
        canvas.SetActive(!canvas.activeSelf);
        if (canvas.activeSelf)
        {
            if (buttonsCreated)
            {
                selectButton();
            }
        }
        else
        {
            //unhighlight all units
            BattleSystem.instance.setHighlights();

            //delete this menu
            Destroy(gameObject);
        }
    }

    void spawnOrderButton(Vector3 position, Unit unit, int playerNum)
    {
        //spawn a target button above the unit
        Vector3 buttonPosition = position;
        buttonPosition.y += 2f;//in the future, this may be unique to the unit
        Button button = Instantiate(orderButtonPrefab, buttonPosition, Quaternion.identity, canvas.transform);

        //change button's held members
        button.GetComponent<orderButton>().setButton(unit, playerNum, GetComponent<AnalyzeMenuControl>());
    }
    //used to sort units in units list
    int compareUnits(Unit a, Unit b)
    {
        if (a == null || b == null)
        {
            return 0;
        }

        int aSpeed = a.speed;
        int bSpeed = b.speed;

        return aSpeed.CompareTo(bSpeed);
    }

    public void spawnInfoPanel(Unit unit)
    {
        //destroy the old panel if it exists
        if (infoPanelInstance != null)
        {
            Destroy(infoPanelInstance.gameObject);
        }

        //create the new one
        infoPanelPrefab.gameObject.SetActive(false);//so I can set unit before start is called
        infoPanelInstance = Instantiate(infoPanelPrefab, canvas.transform);
        infoPanelInstance.setUnit(unit);
        infoPanelInstance.gameObject.SetActive(true);
    }
}
