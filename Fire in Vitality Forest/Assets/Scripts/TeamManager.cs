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
    public List<GameObject> playerGOs;//holds current sprites and unit data
    public List<PlayerUnit> team;//unit data from playerGOs. should update in playerGOs when changed

    void Start()
    {
        for (int index = 0; index<playerPrefabs.Count; index++)
        {
            //create instances of each prefab as child to GameController, and add reference to playerGOs
            GameObject thisPlayer = (GameObject)Instantiate(playerPrefabs[index], this.transform);
            playerGOs.Add(thisPlayer);
            thisPlayer.SetActive(false); //Deactivate this game object since it does nothing
            PlayerUnit thisUnit = thisPlayer.GetComponent<PlayerUnit>();
            team.Add(thisUnit);
        }
    }

    public void setTeam(List<GameObject> newPlayerGOs)
    {
        deleteTeam();
        playerGOs = newPlayerGOs;
        for (int index = 0; index<playerGOs.Count; index++)
        {
            PlayerUnit thisUnit = playerGOs[index].GetComponent<PlayerUnit>();
            team.Add(thisUnit);
        }

    }

    void deleteTeam()
    {//removes the team in this. Only done just before creating a new one
        team.Clear();
        foreach (GameObject player in playerGOs)
        {
            Destroy(player);
        }
    }

    public List<GameObject> getPlayerGOs()
    {
        return playerGOs;
    }

    public List<PlayerUnit> getTeam()
    {
        return team;
    }

    public Element getFirstPlayerColor()
    {
        if (team.Count == 0)
        {
            Debug.Log("Warning: no team in team manager");
        }
        return team[0].getColor();
    }
}
