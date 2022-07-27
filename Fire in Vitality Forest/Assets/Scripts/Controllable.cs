using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controllable : MonoBehaviour
{
    public bool hasControl;
    public int menuDepth;

    public virtual void switchControl()
    {
        hasControl = !hasControl;
    }

    public virtual int getMenuDepth()
    {
        return menuDepth;
    }

    public virtual void changeAble()
    {
        //will disable UI elements if applicable
    }

    public virtual void changeActive()
    {
        //will deactivate canvas if applicable
    }
}
