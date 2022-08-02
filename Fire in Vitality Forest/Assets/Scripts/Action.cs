using UnityEngine;

public class Action : ScriptableObject
{//created when a unit's move has been decided and is to be added to the list
    public Skill skill;
    public Unit user;
    public Unit[] targets;

    public void setAction(Skill _skill, Unit _user, Unit[] _targets)
    {
        skill = _skill;
        user = _user;
        targets = _targets;
    }
    
    public int getUserSpeed()
    {
        return user.speed;
    }

    public int getSkillIndex()
    {
        return skill.index;
    }

    public Unit getUser()
    {
        return user;
    }

    public Unit[] getTargets()
    {
        return targets;
    }
}
