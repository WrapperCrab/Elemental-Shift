using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresetBattle : MonoBehaviour
{
    //Each battle has a list of enemies, background, and terrain effects. For now, it just has enemies
    public List<GameObject> enemyPrefabs;

    public List<GameObject> getEnemyPrefabs()
    {
        return enemyPrefabs;
    }
}
