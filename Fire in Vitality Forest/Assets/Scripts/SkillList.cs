using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SkillList : MonoBehaviour
{
    #region Singleton

    public static SkillList instance;//find inventory with Inventory.instance
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of SkillList found!");
            return;
        }
        instance = this;
    }

    #endregion

    //contains a list of every skill's action

    public void performAction(Action action)//targets is length 1 if only 1 target
    {
        int index = action.getSkillIndex();
        Unit user = action.getUser();
        Unit[] targets = action.getTargets();
        switch (index)
        {
            case 0:
            default:
                //pass
                return;
            case 1:
                attack(user, targets[0]);
                return;
        }
    }

    void attack(Unit user, Unit target)
    {
        int attack = user.attack;
        int defense = user.defense;
        int damage = (int)Math.Ceiling(((double)attack / (double)defense));

        target.takeDamage(damage);
    }
}
