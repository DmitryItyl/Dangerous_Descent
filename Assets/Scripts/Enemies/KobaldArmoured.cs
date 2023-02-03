using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KobaldArmoured : EnemyBase
{
    [SerializeField] float kobaldArmouredSpeed;
    [SerializeField] float kobaldArmouredAggroDistance;
    [SerializeField] int kobaldArmouredHealth;
    [SerializeField] float kobaldArmouredAttackRate;

    [SerializeField] private int kobaldArmouredDamage;

    private void Awake()
    {
        base.Start();
        damage = kobaldArmouredDamage;
        speed = kobaldArmouredSpeed;
        aggroDistance = kobaldArmouredAggroDistance;
        maxHealth = kobaldArmouredHealth;
    }
/*
    void Attack()
    {
        if (reachedTarget)
        {
            if (animator.GetInteger("State") != 2)
                StartCoroutine(MeleeAttack());
            animator.SetInteger("State", 2);
        }
        else
            animator.SetInteger("State", 1);
    }

    IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(kobaldArmouredAttackRate * 0.7f);
        while (reachedTarget)
        {
            //DealDamage();
            Debug.Log("Rawr");
            yield return new WaitForSeconds(kobaldArmouredAttackRate);
        }

        yield return null;
    }
*/
}
