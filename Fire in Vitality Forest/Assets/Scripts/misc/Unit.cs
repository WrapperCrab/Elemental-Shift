using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public enum Highlight {NONE, TARGETTED, DEAD, ACTING}
public enum Affinity {WEAK, NORMAL, STRONG}
public enum Element {k, R, G, B, c, y, m}

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


    //affinities
    public Affinity[] weaknesses = new Affinity[5];//none, water, earth, fire, air

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
        sprite.color = getColorHue();
    }

    public void updateColor()
    {
        sprite.color = getColorHue();
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
        return color.ToString();
    }

    public string getColorName()
    {//returns string name of current color
        switch (color)
        {
            case (Element.k):
            default:
                return "Black";
            case (Element.R):
                return "Red";
            case (Element.G):
                return "Green";
            case (Element.B):
                return "Blue";
            case (Element.c):
                return "Cyan";
            case (Element.y):
                return "Yellow";
            case (Element.m):
                return "Magenta";
        }
    }

    public Color getColorHue()
    {//returns current imbued color of player
        switch (color)
        {
            case (Element.k):
            default:
                return Color.black;
            case (Element.R):
                return Color.red;
            case (Element.G):
                return Color.green;
            case (Element.B):
                return Color.blue;
            case (Element.c):
                return Color.cyan;
            case (Element.y):
                return Color.yellow;
            case (Element.m):
                return Color.magenta;
        }
    }

    public void scaleSprite()
    {//scales the sprite and outline based on height
        float scaleFactor = height / sprite.bounds.size.x;
        sprite.size = new Vector2(scaleFactor*sprite.bounds.size.x, scaleFactor*sprite.bounds.size.y);
        outline.size = new Vector2(scaleFactor * outline.bounds.size.x, scaleFactor * outline.bounds.size.y);
    }
}
