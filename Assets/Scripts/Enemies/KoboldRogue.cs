using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldRogue : EnemyBase
{
    [SerializeField] float koboldRogueSpeed;
    [SerializeField] float koboldRogueAggroDistance;
    [SerializeField] int koboldRogueHealth;
    [SerializeField] float koboldRogueAttackRate;
    [SerializeField] float koboldRogueAttackReach;

    [SerializeField] private int koboldRogueDamage;

    private void Awake()
    {
        base.Start();
        damage = koboldRogueDamage;
        speed = koboldRogueSpeed;
        aggroDistance = koboldRogueAggroDistance;
        maxHealth = koboldRogueHealth;
        attackRate = koboldRogueAttackRate;
        reach = koboldRogueAttackReach;
    }
}
