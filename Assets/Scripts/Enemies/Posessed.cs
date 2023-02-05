using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Posessed : EnemyBase
{
    [SerializeField] float posessedSpeed;
    [SerializeField] float posessedAggroDistance;
    [SerializeField] int posessedHealth;
    [SerializeField] float posessedAttackRate;
    [SerializeField] float posessedAttackReach;

    [SerializeField] private int posessedDamage;
    [SerializeField] GameObject pickAxePrefab;
    [SerializeField] float projectileForce;

    private Transform pickAxeSpawnPoint;

    private void Awake()
    {
        base.Start();
        damage = posessedDamage;
        speed = posessedSpeed;
        aggroDistance = posessedAggroDistance;
        maxHealth = posessedHealth;
        attackRate = posessedAttackRate;
        reach = posessedAttackReach;

        pickAxeSpawnPoint = transform.Find("AxeSpawningPoint");
    }

    // This enemy fights with ranged attacks even though the method is called melee attack
    protected override void StartMeleeAttack()
    {
        ChangeAnimationState(ENEMY_ATTACK);
        agent.speed *= attackSlowDownModifier;
        StartCoroutine(ThrowPickAxe());
    }

    public override void TakeDamage(int takenDamage)
    {
        if (isAlive)
        {
            healthBar.SetHealth(currentHealth, maxHealth);
            currentHealth = Mathf.Clamp(currentHealth - takenDamage, 0, maxHealth);
            if (currentHealth <= 0)
            {
                isAlive = false;
                healthBar.TurnOff();
                return;
            }

            StartCoroutine(DamageRecieve());
        }
    }

    protected override IEnumerator DamageRecieve()
    {
        agent.speed *= hitSlowDownModifier;
        yield return new WaitForSeconds(attackRate * 0.25f);
        agent.speed /= hitSlowDownModifier;
    }

    private IEnumerator ThrowPickAxe()
    {
        while (isAttacking && isAlive)
        {
            yield return new WaitForSeconds(attackRate * 0.72f);
            Vector3 playerPos = player.transform.position;

            Vector3 aimDirection = (playerPos - transform.position).normalized;
            var aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
            float aimAngleDeg = aimAngle * Mathf.Rad2Deg;
            pickAxeSpawnPoint.transform.eulerAngles = new Vector3(0, 0, aimAngleDeg);

            GameObject bullet = Instantiate(pickAxePrefab, pickAxeSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            float xComponent = Mathf.Cos(aimAngle) * projectileForce;
            float zComponent = Mathf.Sin(aimAngle) * projectileForce;

            Vector2 forceApplied = new Vector2(xComponent, zComponent);
            rb.AddForce(forceApplied, ForceMode2D.Impulse);

            yield return new WaitForSeconds(attackRate * 0.28f);
        }

        AttackComplete();
    }

    new void AttackComplete()
    {
        agent.speed /= attackSlowDownModifier;
        ChangeAnimationState(ENEMY_RUN);

        isAttacking = false;
    }
}
