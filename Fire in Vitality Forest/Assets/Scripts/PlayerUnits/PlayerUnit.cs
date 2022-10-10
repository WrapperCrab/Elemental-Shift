using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public int currentM;
    public int maxM;

    public Action normalAttack;

    public void setValues(PlayerUnit _player)//!!!In the near future, I will stop using this function. player game objects will already exist in TeamManagert
    {
        unitName = _player.unitName;
        currentH = _player.currentH;
        maxH = _player.maxH;
        attack = _player.attack;
        defense = _player.defense;
        speed = _player.speed;
        color = _player.color;
        weaknesses = _player.weaknesses;
        skills = _player.skills;
        description = _player.description;
        height = _player.height;
        currentM = _player.currentM;
        maxM = _player.maxM;
    }



}
