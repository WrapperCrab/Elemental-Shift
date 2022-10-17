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

    public bool getAttacked(int _attack)//this will have more inputs later
    {
        int damage = (int)Math.Ceiling(((double)_attack/(double)defense));

        currentH -= damage;

        if (currentH <= 0)
        {
            return true;//unit has died
        }
        else
        {
            return false;
        }
    }

    public bool takeDamage(int damage)
    {
        currentH -= damage;
        currentH = Math.Max(currentH, 0);
        if (currentH == 0)
        {
            return true;//unit has died
        }
        else
        {
            return false;
        }
    }

    public void gainHealth(int heal)
    {
        currentH += heal;
        currentH = Math.Min(currentH, maxH);
    }

    public void setColor(Element newColor)
    {//changes the imbued color of the unit
        color = newColor;
        updateColor();
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
    {
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
}
