using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnalyzeMenuControl : MenuControl
{
    public Button orderButtonPrefab;//these buttons do nothing when clicked, but spawn panel when selected
    public GameObject infoPanelPrefab;//displays unit info

    public List<Button> orderButtons = new List<Button>();//for setting firstButton after all buttons are spawned
    List<Unit> units = new List<Unit>();

    bool buttonsCreated = false;

    public override void Start()
    {
        units.AddRange(BattleSystem.instance.enemies);
        units.AddRange(BattleSystem.instance.team);

        foreach (Unit unit in units)
        {
            //create button above unit
            spawnOrderButton(unit.GetComponent<Transform>().position, unit);
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

    void spawnOrderButton(Vector3 position, Unit unit)
    {
        //spawn a target button above the unit
        Vector3 buttonPosition = position;
        buttonPosition.y += 2f;//in the future, this may be unique to the unit
        Button button = Instantiate(orderButtonPrefab, buttonPosition, Quaternion.identity, canvas.transform);

        //change button's held members
        button.GetComponent<orderButton>().setButton(unit);
    }

    void orderButtonText()
    {//called when all buttons are spawned. changes text of button to numbered order in turn

    }
}
