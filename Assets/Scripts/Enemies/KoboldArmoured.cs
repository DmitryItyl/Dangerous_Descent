using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldArmoured : EnemyBase
{
    [SerializeField] float kobaldArmouredSpeed;
    [SerializeField] float kobaldArmouredAggroDistance;
    [SerializeField] int kobaldArmouredHealth;
    [SerializeField] float kobaldArmouredAttackRate;
    [SerializeField] float kobaldArmouredAttackReach;

    [SerializeField] private int kobaldArmouredDamage;

    private void Awake()
    {
        base.Start();
        damage = kobaldArmouredDamage;
        speed = kobaldArmouredSpeed;
        aggroDistance = kobaldArmouredAggroDistance;
        maxHealth = kobaldArmouredHealth;
        attackRate = kobaldArmouredAttackRate;
        reach = kobaldArmouredAttackReach;
    }

}
