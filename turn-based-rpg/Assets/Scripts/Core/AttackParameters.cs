using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackParameters
{
    public CharacterBattle attacker;
    public CharacterBattle target;
    public bool missed;
    public bool crit;
    public float damage;
    public Action onHit;
}
