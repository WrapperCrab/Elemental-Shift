using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerInteract : Interaction
{
    public Element color;
    public SpriteRenderer fill;

    void Start()
    {
        setColor(color);//makes visual color match script color
    }

    void setColor(Element newColor)
    {
        color = newColor;
        fill.color = ElementManager.instance.getColorHue(color);
    }

    public override void interact()
    {
        triggerDialogue();
        imbueColor();
    }

    public void imbueColor()
    {
        TeamManager.instance.getTeam()[0].setColor(color);
    }
}
