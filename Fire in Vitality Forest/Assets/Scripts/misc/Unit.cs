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
    public ImbuedElement element;

    //affinities
    public Affinity[] weaknesses = new Affinity[5];//none, water, earth, fire, air

    public List<Action> skills;//These are added in the inspector

    public string description;

    public int height;//used for scaling sprite
    public SpriteRenderer sprite;
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

    public void setHighlight(Highlight _highlight)
    {
        switch (_highlight)
        {
            case Highlight.TARGETTED://green
                sprite.color = new Color(0,1,0,1);
                break;
            case Highlight.ACTING://blue
                sprite.color = new Color(0, 0, 1, 1);
                break;
            case Highlight.DEAD://red
                sprite.color = new Color(1, 0, 0, 1);
                break;
            case Highlight.NONE://white
            default:
                sprite.color = new Color(1, 1, 1, 1);
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
}
