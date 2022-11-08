using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Highlight {NONE, TARGETTED, DEAD, ACTING}
public enum Affinity {WEAK, NORMAL, STRONG}

public abstract class Unit : MonoBehaviour
{
    public string unitName;
    //public int unitLevel;

    //stats
    public int currentH;
    public int maxH;
    public int attack;
    public int defense;
    public int speed;
    public Element color;


    public Affinity[] weaknesses = new Affinity[7];

    public List<Action> skills;//These are added in the inspector

    public string description;

    public int height;//used for scaling sprite
    public SpriteRenderer sprite;
    public SpriteRenderer outline;


    protected Highlight highlight = Highlight.NONE;

    public bool takeDamage(int damage)
    {
        //deal with H
        currentH -= damage;
        currentH = Math.Max(currentH, 0);
        if (currentH == 0)
        {
            //do "dying" animation 

            //Trigger damage popup above head
            damagePopup(damage, false);
            return true;//unit has died
        }
        else
        {
            //do "attacked" animation

            //Trigger damage popup above head
            damagePopup(damage, false);
            return false;
        }

        
    }

    public bool gainHealth(int heal)
    {//fails if unit is dead
        if (currentH != 0)
        {
            //deal with H
            currentH += heal;
            currentH = Math.Min(currentH, maxH);

            //do "healing" animation

            //trigger damage popup above head
            damagePopup(heal, true);

            return true;
        }
        return false;
    }

    public bool revive(int newHealth)
    {//fails if unit is not dead
        if (currentH == 0)
        {
            //change unit data
            currentH = newHealth;

            //do "reviving" animation

            //trigger damage popup above head
            damagePopup(newHealth, true);

            return true;
        }
        return false;
    }

    public void setColor(Element newColor)
    {//changes the imbued color of the unit
        color = newColor;
        updateColor();//absorb animation done here
    }

    public Element getColor()
    {
        return color;
    }

    public void combineColor(Element newColor)
    {//combines the new color with the current color
        Element combinedColor = ElementManager.instance.combineColors(color, newColor);
        setColor(combinedColor);
    }

    public void updateColor()
    {//!!!
        //do "absorb" animation
        sprite.color = ElementManager.instance.getColorHue(color);
    }

    public void setHighlight(Highlight _highlight)
    {
        switch (_highlight)
        {
            case Highlight.TARGETTED://green
                outline.color = new Color(0,1,0,1);
                break;
            case Highlight.ACTING://blue
                outline.color = new Color(0, 0, 1, 1);
                break;
            case Highlight.DEAD://red
                outline.color = new Color(1, 0, 0, 1);
                break;
            case Highlight.NONE://white
            default:
                outline.color = new Color(1, 1, 1, 1);
                break;
        }
        highlight = _highlight;
    }

    public string getAffinityAbrev(Affinity affinity)
    {
        switch (affinity)
        {
            case Affinity.WEAK:
                return "we";
            case Affinity.NORMAL:
            default:
                return "-";
            case Affinity.STRONG:
                return "str";
        }
    }

    public string getColorAbrev()
    {//returns abbreviation of current color
        return ElementManager.instance.elementDict[color].Item1;
    }

    public string getColorName()
    {//returns string name of current color
        return ElementManager.instance.elementDict[color].Item2;
    }

    public void scaleSprite()
    {//scales the sprite and outline based on height
        float scaleFactor = height / sprite.bounds.size.x;
        sprite.size = new Vector2(scaleFactor*sprite.bounds.size.x, scaleFactor*sprite.bounds.size.y);
        outline.size = new Vector2(scaleFactor * outline.bounds.size.x, scaleFactor * outline.bounds.size.y);
    }

    public void damagePopup(int damage, bool isHeal)
    {
        DamagePopup damagePopupPrefab = TeamManager.instance.damagePopupPrefab;
        Vector2 thisPosition = gameObject.transform.position;
        float popupPositionX = thisPosition.x;
        float popupPositionY = thisPosition.y + 2;//should consider height?
        Vector2 popupPosition = new Vector2(popupPositionX, popupPositionY);
        DamagePopup dp = Instantiate(damagePopupPrefab, popupPosition, Quaternion.identity);
        if (isHeal)
        {
            dp.setup(damage, Color.green);
        }
        else
        {
            dp.setup(damage, Color.red);
        }
    }
}
