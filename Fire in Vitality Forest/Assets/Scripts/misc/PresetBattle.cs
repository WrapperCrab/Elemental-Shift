using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetBattle : MonoBehaviour
{
    //Each battle has a list of enemies, background, and terrain effects.
    public List<GameObject> enemyPrefabs;
    public List<GameObject> playerGOs;//references to GOs in GameController. Never stop existing
    public List<PlayerUnit> team;

    public List<GameObject> getEnemyPrefabs()
    {
        return enemyPrefabs;
    }

    public List<GameObject> getPlayerGOs()
    {
        return playerGOs;
    }

    public void setPlayerPrefabs(List<GameObject> _playerGOs)
    {
        playerGOs = _playerGOs;
    }

    public void setTeam(List<PlayerUnit> _team)
    {
        team = _team;
    }
}
