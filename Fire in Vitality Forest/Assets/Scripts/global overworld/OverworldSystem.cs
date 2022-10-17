using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class OverworldSystem : MonoBehaviour
{
    #region Singleton

    public static OverworldSystem instance;
    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of OverworldSystem found!");
            return;
        }
        instance = this;
    }

    #endregion

    /*This script will keep track of the battle triggers in an area
     as well as anything which can only be activated a finite number of times
    */
    public BattleTrigger[] battleTriggers;

    public void deactivateTrigger(int index)
    {
        battleTriggers[index].deactivate();
    }

    public int getIndex(BattleTrigger battleTrigger)
    {
        return Array.IndexOf(battleTriggers, battleTrigger);
    }
}
