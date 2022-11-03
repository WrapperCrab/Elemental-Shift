using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    public virtual Action selectAction()
    {//used by enemies to select their move
        return null;//null is treated as pass
    }

    public virtual bool absorbAction()
    {//used by enemy to decide whether or not to absorb an attack
        return false;
    }
}
