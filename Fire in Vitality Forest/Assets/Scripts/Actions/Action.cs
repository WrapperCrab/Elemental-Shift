using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Action", menuName = "Actions/New Action")]
public class Action : ScriptableObject
{//created when a unit's move has been decided and is to be added to the list
    new public string name = "new skill";
    public string description = "new skill description";
    public int mCost = 0;

    public bool onEnemy = true;//true if useable on enemy
    public bool onTeam = false;//true if useable on teammate(s)
    public bool hitsAll = false;//true if affects all teammates or enemies at once
    public bool selfMove = false;//true if it only can affect the user

    public Element color = Element.k;//color of this action. Usually set to the color of the user
    public bool absorbable = true;//true if it can be absorbed when it is successful

    public bool usableOutsideBattle = false;

    //These attributes are only nondefault during battle
    protected Unit user;
    protected List<Unit> targets = new List<Unit>();

    public virtual void performAction()
    {//effects on units in battle due to this move

    }

    public virtual string moveCompletedText()
    {//text displayed after this move was used
        return null;
    }

    public void setAction(Action action)//only used before targets and users are set
    {
        name = action.name;
        description = action.description;
        mCost = action.mCost;

        onEnemy = action.onEnemy;
        onTeam = action.onTeam;
        hitsAll = action.hitsAll;
        selfMove = action.selfMove;

        usableOutsideBattle = action.usableOutsideBattle;

    }

    public void setUser(Unit _user)
    {
        user = _user;
    }
    public Unit getUser()
    {
        return user;
    }
    public bool getUserDead()
    {
        return (user.currentH <= 0);
    }

    public void setTargets(List<Unit> _targets)
    {
        targets = _targets;
    }
    public List<Unit> getTargets()
    {
        return targets;
    }

    public void addTarget(Unit _target)
    {
        targets.Add(_target);
    }
    public void removeTarget(Unit _target)
    {
        targets.Remove(_target);
    }

    public int getMCost()
    {
        return mCost;
    }
    
    public int getUserSpeed()
    {
        return user.speed;
    }

    public int getTargetType()
    {//8 target types defined by 4 bools altogether
        int targetType = 0;
        if (onEnemy)
        {
            targetType += 4;
        }
        if (onTeam)
        {
            targetType += 2;
        }
        if (hitsAll)
        {
            targetType++;
        }
        if (selfMove)
        {
            targetType--;
        }
        return targetType;
    }

    public bool getHitsAll()
    {
        return hitsAll;
    }
    public bool getOnEnemy()
    {
        return onEnemy;
    }
    public bool getOnTeam()
    {
        return onTeam;
    }
    public bool hasNoTarget()
    {
        return (!onEnemy && !onTeam);
    }

    public bool getAbsorbable()
    {
        return absorbable;
    }


    public Element getColor()
    {
        return color;
    }
    public virtual void updateColor()
    {
        color = user.getColor();
        //some moves have inherent colors independent of user and will override this method
    }

    public bool getInsufficientM()
    {
        //find if user is player
        PlayerUnit convertedUser = user as PlayerUnit;
        if (convertedUser == null)
        {//this is not a player
            return false;
        }
        else
        {
            return (convertedUser.currentM < mCost);
        }        
    }

    public bool getAllTargetsDead()
    {
        bool allDead = true;
        foreach (Unit target in targets)
        {
            if (target.currentH > 0)
            {
                allDead = false;
                break;
            }
        }
        return allDead;
    }
    public bool removeAllDeadTargets()
    {
        targets.RemoveAll(target => target.currentH <= 0);
        if (targets.Count == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void spendM()//This is only called if the user has enough magic to use the move
    {
        //find if user is player
        PlayerUnit convertedUser = user as PlayerUnit;
        if (convertedUser != null)
        {//this is a player
            convertedUser.currentM -= mCost;
        }
    }
}
