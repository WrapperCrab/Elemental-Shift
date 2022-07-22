using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public bool hasControl;

    public virtual void switchControl()
    {
        hasControl = !hasControl;
    }
}
