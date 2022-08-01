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
    public int currentM;
    public int maxM;

    public int attack;
    public int defense;
    public int speed;

    public ImbuedElement element;

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
}
