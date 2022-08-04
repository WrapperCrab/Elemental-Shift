using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "New Action", menuName = "Actions/New Action")]
public class Action : ScriptableObject
{//created when a unit's move has been decided and is to be added to the list
    new public string name = "new skill";
    public string description = "new skill description";
    public int index = 0;//unique for each skill

    public bool onEnemy = true;//true if useable on enemy
    public bool onTeam = false;//true if useable on teammate(s)
    public bool hitsAll = false;//true if affects all teammates or enemies at once
    public bool selfMove = false;//true if it only can affect the user

    public bool usableOutsideBattle = false;

    //These attributes are only nondefault during battle
    public Unit user;
    public List<Unit> targets;

    public void setUser(Unit _user)
    {
        user = _user;
    }

    public void setTargets(List<Unit> _targets)
    {
        targets = _targets;
    }
    
    public int getUserSpeed()
    {
        return user.speed;
    }

    public int getSkillIndex()//!!!I don't think I want the index anymore
    {
        return index;
    }

    public Unit getUser()
    {
        return user;
    }

    public List<Unit> getTargets()
    {
        return targets;
    }

    public void removeTarget(int index)
    {
        targets.RemoveAt(index);
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

    public bool hasNoTarget()
    {
        return (!onEnemy && !onTeam);
    }

    public void setAction(Action action)//only used before targets and users are set
    {
        name = action.name;
        description = action.description;
        index = action.index;//unique for each skill

        onEnemy = action.onEnemy;
        onTeam = action.onTeam;
        hitsAll = action.hitsAll;
        selfMove = action.selfMove;

        usableOutsideBattle = action.usableOutsideBattle;
    }
 

    public virtual void performAction()//this was previously stored in SkillList
    {//effects on units in battle due to this move

    }

    public virtual string moveCompletedText()
    {//text displayed after this move was used
        return null;
    }
}
