using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected float speed;
    protected float aggroDistance;

    protected int damage;
    protected float attackRate;

    protected float reach;

    private int currentHealth;
    protected int maxHealth;

    protected Animator animator;
    protected Rigidbody2D rb2D;

    protected Transform player;

    protected NavMeshAgent agent;

    bool isAttacking;
    private float attackSlowDownModifier = 0.2f;
    private float hitSlowDownModifier = 0.05f;

    protected bool isAggro = false;
    protected bool facesRight = true;
    protected bool isAlive = true;

    string currentState;

    // Animation states
    protected const string ENEMY_ATTACK = "Attack";
    protected const string ENEMY_RUN = "Run";
    protected const string ENEMY_IDLE = "Idle";
    protected const string ENEMY_DEATH = "Death";
    protected const string ENEMY_HIT = "Hit";

    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        agent.speed = speed;

        currentState = ENEMY_IDLE;
    }

    protected void FixedUpdate()
    {
        if (isAlive)
        {
            if (!isAggro)
            {
                CheckAggro();
                return;
            }

            FollowTarget();

            if (agent.remainingDistance <= reach)
            {
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartMeleeAttack();
                }
            }
            else
            {
                isAttacking = false;
            }
        }
        else
            Die();
    }

    #region Movement

    protected void FollowTarget()
    {
        float cornerPlayerDirection = Mathf.Sign(transform.position.x - player.position.x);
        Vector3 goToPosition = new Vector3(player.position.x + 0.4f * cornerPlayerDirection, player.position.y, player.position.z);

        agent.SetDestination(goToPosition);
        Debug.Log(agent.speed);
        ControlDirection();
    }

    protected void ControlDirection()
    {
        float xVel = agent.velocity.x;
        bool direction = xVel > 0;

        if (xVel != 0)
        {
            Flip(direction);
        }
        else
        {
            float playerDirection = player.position.x - transform.position.x;
            if (playerDirection != 0)
                Flip(playerDirection > 0);
        }
            return;
    }

    protected void Flip(bool direction)
    {
        if (facesRight == direction)
            return;

        facesRight = direction;
        Vector3 theScale = transform.localScale;

        if (!direction)
            theScale.x = -Mathf.Abs(theScale.x);
        else
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }

    #endregion

    protected void CheckAggro()
    {
        if (!isAggro && agent.remainingDistance <= aggroDistance)
        {
            isAggro = true;
            ChangeAnimationState(ENEMY_RUN);
            FollowTarget();
        }
    }

    protected void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Axe")
        {
            var damageTaken = player.GetComponent<PlayerCotroller>().DeliverDamage();
            TakeDamage(damageTaken);
        }
    }

    public void TakeDamage(int takenDamage)
    {
        currentHealth = Mathf.Clamp(currentHealth - takenDamage, 0, maxHealth);
        if (currentHealth <= 0)
        {
            isAlive = false;
            return;
        }

        ChangeAnimationState(ENEMY_HIT);
        StartCoroutine(DamageRecieve());

        Debug.Log("Enemy health: " + currentHealth);
    }

    protected IEnumerator DamageRecieve()
    {
        agent.speed *= hitSlowDownModifier;
        yield return new WaitForSeconds(attackRate * 0.25f);
        agent.speed /= hitSlowDownModifier;

        if (isAttacking)
            ChangeAnimationState(ENEMY_ATTACK);
        else
            ChangeAnimationState(ENEMY_RUN);
    }


    protected void Die()
    {
        Debug.Log("Dead");
        animator.Play(ENEMY_DEATH);
        agent.isStopped = true;
        Destroy(gameObject, 4f);
    }

    protected void StartMeleeAttack()
    {
        ChangeAnimationState(ENEMY_ATTACK);
        agent.speed *= attackSlowDownModifier;
        StartCoroutine(MeleeAttack());
    }

    protected void DealDamage()
    {
        player.GetComponent<PlayerCotroller>().TakeDamage(damage);
    }

    protected void AttackComplete()
    {
        agent.speed /= attackSlowDownModifier;
        ChangeAnimationState(ENEMY_RUN);
    }

    protected IEnumerator MeleeAttack()
    {
        yield return new WaitForSeconds(attackRate * 0.7f);
        while (isAttacking)
        {
            DealDamage();            

            yield return new WaitForSeconds(attackRate);
        }

        AttackComplete();
    }

    protected void ChangeAnimationState(string newState)
    {
        if (currentState == newState)
            return;

        animator.Play(newState);

        currentState = newState;
    }
}
