using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBattleStation : MonoBehaviour
{
    public bool filled = false;

    public void deleteIfDead()
    {
        if (filled)
        {
            GameObject enemy = gameObject.transform.GetChild(0).gameObject;
            if (enemy.GetComponent<EnemyUnit>().currentH <= 0)
            {//delete this enemy
                clearStation();
            }
        }
    }

    public void clearStation()
    {
        if (filled)
        {
            Destroy(gameObject.transform.GetChild(0).gameObject);
        }
        else
        {
            Debug.Log("station Cleared when already empty");
        }
        filled = false;
    }

    public GameObject fillStation(GameObject enemyPrefab)
    {
        Debug.Log("filling");
        if (!filled)
        {
            filled = true;
            return Instantiate(enemyPrefab, gameObject.GetComponent<Transform>());
        }
        else
        {
            Debug.Log("station filled when already full!!!");
            return null;
        }
    }
}
