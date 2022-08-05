using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public virtual Action selectAction()
    {//used by enemies to select their move
        return pass;
    }
}
