using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeamManager : MonoBehaviour
{
    #region Singleton

    public static TeamManager instance;
    void Awake()
    {
        if (instance != null)
        {
            //Debug.LogWarning("More than one instance of TeamManager found!");
            return;
        }
        instance = this;
    }

    #endregion

    //This script keeps track of the current party and their stats
    public List<GameObject> playerPrefabs;//holds sprites and default unit data
    public List<PlayerUnit> team;//holds stats, moves, etc. Will be added to copy of prefab

    void Start()//!!!This function is stupid
    {
        for (int index = 0; index<playerPrefabs.Count; index++)
        {//create player unit object
            team.Add(playerPrefabs[index].GetComponent<PlayerUnit>());
        }
    }

    public List<GameObject> getPlayerGOs()
    {
        List<GameObject> playerGOs = new List<GameObject>();
        for (int index = 0; index<playerPrefabs.Count; index++)
        {//combine prefabs and units
            GameObject player = playerPrefabs[index];//load prefab

            player.GetComponent<PlayerUnit>().setValues(team[index]);//set to new unit's values

            playerGOs.Add(player);//add to final list
        }

        return playerGOs;
    }
}
