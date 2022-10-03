using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BattleHUD : MonoBehaviour
{

    public TextMeshProUGUI nameText;
    public Slider hSlider;
    public TextMeshProUGUI currentHealth;

    public Slider mSlider;
    public TextMeshProUGUI currentMagic;

    public TextMeshProUGUI color;


    public void setHUD(PlayerUnit unit)
    {
        nameText.text = unit.unitName;

        hSlider.maxValue = unit.maxH;
        hSlider.value = unit.currentH;
        currentHealth.text = unit.currentH + "/" + unit.maxH;
        
        mSlider.maxValue = unit.maxM;
        mSlider.value = unit.currentM;
        currentMagic.text = unit.currentM + "/" + unit.maxM;

        color.text = unit.getColorName();
    }

    public void setH(int h)
    {
        hSlider.value = h;
    }

    public void setM(int m)
    {
        mSlider.value = m;
    }

    public void setColor(string colorName)
    {
        color.text = colorName;
    }


}
