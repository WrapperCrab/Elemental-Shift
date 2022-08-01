using UnityEngine;

[CreateAssetMenu(fileName = "New Item", menuName = "Skill")]
public class Skill : ScriptableObject
{
    public string skillName = "new skill";
    public string description = "new description";
    public int skillIndex = 0;//unique for each skill

    public bool onEnemy;//true if used on enemy
    public bool onTeam;//true if used on teammate(s)

    public bool hitsAll;//true if affects all teammates or enemies at once
    public bool usesImbue;//true if effect changes depending on user's imbued element


    //how do I add the different effects a skill has?
}
