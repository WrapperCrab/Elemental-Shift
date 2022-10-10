using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleInitializer : MonoBehaviour
{
    #region Singleton

    public static BattleInitializer instance;
    void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of BattleInitializer found!");
            return;
        }
        instance = this;
    }

    #endregion

    PresetBattle battle;

    public void initBattle(PresetBattle _battle)
    {
        battle = _battle;
        SceneManager.LoadScene("Battle");
    }

    public PresetBattle getBattle()
    {
        return battle;
    }
}
