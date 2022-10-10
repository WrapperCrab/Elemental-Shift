using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetBattle : MonoBehaviour
{
    //Each battle has a list of enemies, background, and terrain effects. For now, it just has enemies
    public List<GameObject> enemyPrefabs;
    public List<GameObject> playerPrefabs;
    public List<PlayerUnit> team;

    public List<GameObject> getEnemyPrefabs()
    {
        return enemyPrefabs;
    }

    public List<GameObject> getPlayerPrefabs()
    {
        return playerPrefabs;
    }

    public void setPlayerPrefabs(List<GameObject> _playerPrefabs)
    {
        playerPrefabs = _playerPrefabs;
    }

    public void setTeam(List<PlayerUnit> _team)
    {
        team = _team;
    }
}
