using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class infoPanel : MonoBehaviour
{
    Unit unit;
    bool unitIsPlayer;

    // Start is called before the first frame update
    void Start()
    {
        //set every field based on Unit and unitIsPlayer
    }

    public void setUnit(Unit _unit)
    {
        unit = _unit;
        EnemyUnit enemyUnit = _unit as EnemyUnit;
        if (enemyUnit != null)
        {//this is an enemyUnit
            unitIsPlayer = false;
            return;
        }
        //this is a playerUnit
        unitIsPlayer = true;
    }
}
