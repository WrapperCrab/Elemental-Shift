using System;
using System.Collections.Generic;

public class ActionComparer : IComparer<Action>
{
    //used to sort actions in actionsToUse list
    public int Compare(Action a, Action b)
    {
        if (a==null || b == null)
        {
            return 0;
        }
        
        int aUserSpeed = a.getUserSpeed();
        int bUserSpeed = b.getUserSpeed();

        return aUserSpeed.CompareTo(bUserSpeed);
    }
    
}
