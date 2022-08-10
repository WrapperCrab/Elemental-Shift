using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    public string unitName;
    //public int unitLevel;

    public int currentH;
    public int maxH;

    public int attack;
    public int defense;
    public int speed;

    public ImbuedElement element;

    public List<Action> skills;
    public Action pass;

    public SpriteRenderer sprite;
    public bool isHighlighted = false;

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

    public void highlight()
    {
        sprite.color = new Color(0, 1, 0, 1);
        isHighlighted = true;
    }

    public void unHighlight()
    {
        sprite.color = new Color(1, 1, 1, 1);
        isHighlighted = false;
    }
}
