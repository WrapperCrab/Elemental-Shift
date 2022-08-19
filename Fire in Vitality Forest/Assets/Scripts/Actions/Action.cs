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

    public bool usableOutsideBattle = false;

    //These attributes are only nondefault during battle
    protected Unit user;
    protected List<Unit> targets;

    public virtual void performAction()//this was previously stored in SkillList
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

    public bool removeAllDeadTargets()
    {
        targets.RemoveAll(target => target.currentH <= 0);
        if (targets.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
