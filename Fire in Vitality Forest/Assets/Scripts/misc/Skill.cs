using UnityEngine;

[CreateAssetMenu(fileName = "New Skill", menuName = "Skill")]
public class Skill : ScriptableObject
{
    new public string name = "new skill";
    public string description = "new description";

    public int index = 0;//unique for each skill

    //These attributes are used for when the player selects who to use the move on
    public bool onEnemy = true;//true if useable on enemy
    public bool onTeam = false;//true if useable on teammate(s)
    public bool hitsAll = false;//true if affects all teammates or enemies at once
    public bool selfMove = false;//true if it only can affect the user

    public bool usableOutsideBattle = false;

    //skill's effects held in SkillList

}
