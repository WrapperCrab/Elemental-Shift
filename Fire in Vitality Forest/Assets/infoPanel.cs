using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class infoPanel : MonoBehaviour
{
    Unit unit;
    bool unitIsPlayer;

    public TextMeshProUGUI nameText;
    public TextMeshProUGUI hText;
    public TextMeshProUGUI mText;
    public TextMeshProUGUI aText;
    public TextMeshProUGUI dText;
    public TextMeshProUGUI sText;
    public Image elementImage;

    public TextMeshProUGUI affNText;
    public TextMeshProUGUI affWText;
    public TextMeshProUGUI affEText;
    public TextMeshProUGUI affFText;
    public TextMeshProUGUI affAText;

    public TextMeshProUGUI[] skillTexts = new TextMeshProUGUI[3];

    public TextMeshProUGUI descriptionText;

    // Start is called before the first frame update
    void Start()
    {
        disableButtons();//make the buttons in this panel nonInteractable

        //set every field based on Unit and unitIsPlayer
        //destroy relevant fields when applicable
        nameText.text = unit.unitName;
        hText.text = unit.currentH + "/" + unit.maxH;
        aText.text = unit.attack.ToString();
        dText.text = unit.defense.ToString();
        sText.text = unit.speed.ToString();

        affNText.text = unit.getAffinityAbrev(unit.weaknesses[0]);
        affWText.text = unit.getAffinityAbrev(unit.weaknesses[1]);
        affEText.text = unit.getAffinityAbrev(unit.weaknesses[2]);
        affFText.text = unit.getAffinityAbrev(unit.weaknesses[3]);
        affAText.text = unit.getAffinityAbrev(unit.weaknesses[4]);

        int numToDisplay = Math.Min(unit.skills.Count, 3);//can only display up to 3 skills for now
        int skillNum = 0;

        for (skillNum += 0; skillNum<numToDisplay; skillNum++)
        {
            skillTexts[skillNum].text = unit.skills[skillNum].name;
        }
        for (skillNum += 0; skillNum<3; skillNum++)
        {
            //these buttons have no skills destroy them
            Destroy(skillTexts[skillNum].gameObject);
        }


        //mText, elementImage, descriptionText



    }

    public void setUnit(Unit _unit)
    {
        unit = _unit;
        EnemyUnit enemyUnit = _unit as EnemyUnit;
        if (enemyUnit != null)
        {//this is an enemyUnit
            unitIsPlayer = false;
            return;
        }
        //this is a playerUnit
        unitIsPlayer = true;
    }
    void disableButtons()
    {
        foreach (Button button in GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }
}
