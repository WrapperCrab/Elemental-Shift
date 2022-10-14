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
    bool isBattleSet = false;
    public List<BattleTrigger> battleTriggers = new List<BattleTrigger>();//reference to every battle trigger in the overworld.
    int triggeredBattleIndex;//index of most recent battle in battleTriggers
    List<int> activatedTriggers = new List<int>();//list of indeces of activated triggers


    public void initBattle(PresetBattle _battle)
    {
        battle = _battle;
        isBattleSet = true;
        SceneManager.LoadScene("Battle");
    }

    public PresetBattle getBattle()
    {
        return battle;
    }

    public bool getIsBattleSet()
    {
        return isBattleSet;
    }

    void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }
    void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("level finished loading!");
        if (scene.name == "Overworld")
        {
            //deactivate relevant triggers
            foreach (int trigger in activatedTriggers){
                battleTriggers[trigger].gameObject.SetActive(false);
            }
        }
    }

}
