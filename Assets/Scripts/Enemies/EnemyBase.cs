using UnityEngine;
using UnityEngine.AI;

public class EnemyBase : MonoBehaviour
{
    protected float speed;
    protected float aggroDistance;

    protected float damage;

    private int currentHealth;
    protected int maxHealth;

    protected Animator animator;
    private Rigidbody2D rb2D;

    protected Transform player;

    private NavMeshAgent agent;

    protected bool isAggro = false;
    bool facesRight = true;
    protected bool isAlive = true;
    private int deathTimer = 100;


    protected void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        currentHealth = maxHealth;

        animator = GetComponent<Animator>();
        rb2D = GetComponent<Rigidbody2D>();

        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    private void Update()
    {
        agent.SetDestination(player.position);
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            Die();
    }

    #region Movement


    private void MovingManageVisual(float xVel)
    {
        if (xVel < 0 && facesRight)
        {
            Flip();
        }
        if (xVel > 0 && !facesRight)
        {
            Flip();
        }
    }

    private void Flip()
    {
        facesRight = !facesRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    #endregion

    protected void CheckAggro()
    {
        if (Vector3.Distance(transform.position, player.transform.position) <= aggroDistance && !isAggro)
        {
            isAggro = true;
        }
    }

    private void Die()
    {
        animator.SetTrigger("Dead");

        if (deathTimer > 0)
        {
            deathTimer--;
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
