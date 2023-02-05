using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slime : EnemyBase
{
    [SerializeField] float slimeAggroDistance;
    [SerializeField] int slimeHealth;
    [SerializeField] float slimeAttackRate;
    [SerializeField] float slimeAttackReach;
    [SerializeField] float slimeSpeed;

    private bool isSmall = false;

    [SerializeField] private int slimeDamage;

    private void Awake()
    {
        base.Start();
        damage = slimeDamage;
        aggroDistance = slimeAggroDistance;
        maxHealth = slimeHealth;
        attackRate = slimeAttackRate;
        reach = slimeAttackReach;
        speed = slimeSpeed;
    }
}
