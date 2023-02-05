using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotroller : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject axePrefab;
    [SerializeField] float projectileForce;

    [SerializeField] float fireRate = 0.68f;

    [SerializeField] int damageDealt;
    [SerializeField] int maxHealth;

    private int currentHealth;
    private int experience;

    private float xAxis;
    private float yAxis;

    private Animator animator;
    private string currentState;

    private Rigidbody2D rb;
    private bool isFacingRight = true;
    private bool isAttackPressed;
    private bool isAttacking;

    private Transform axeSpawnPoint;
    private float attackSlowDownModifier = 0.2f;

    // Animation states
    const string PLAYER_ATTACK = "Attack";
    const string PLAYER_RUN = "Run";
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_DEATH = "Death";

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        axeSpawnPoint = transform.Find("AxeSpawnPoint");

        currentHealth = maxHealth;
    }

    void Update()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        isAttackPressed = Input.GetButton("Fire1");
    }

    private void FixedUpdate()
    {
        Move(xAxis, yAxis);

        if (isAttackPressed)
        {
            if (!isAttacking)
            {
                isAttacking = true;

                StartShooting();
            }
        }
        else if (isAttacking)
        {
            isAttacking = false;
        }

        if (!isAttacking)
        {
            if ((xAxis != 0) || (yAxis != 0))
            {
                ChangeAnimationState(PLAYER_RUN);
            }
            else
            {
                ChangeAnimationState(PLAYER_IDLE);
            }
        }
    }

    #region Sprites and animations

    private void Flip(float direction)
    {
        bool playerLooksRight = direction > 0;
        if ((direction == 0) || (isFacingRight == playerLooksRight))
            return;

        isFacingRight = playerLooksRight;
        Vector3 theScale = transform.localScale;

        if (!playerLooksRight)
            theScale.x = -Mathf.Abs(theScale.x);
        else
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }

    private void ChangeAnimationState(string newState)
    { 
        if (currentState == newState)
            return;

        animator.Play(newState);
        currentState = newState;
    }

    #endregion

    #region Movement

    private void Move(float horizontal, float vertical)
    {

        var movement = new Vector2(horizontal, vertical).normalized * moveSpeed;

        if (currentState != "Attack")
            Flip(horizontal);

        rb.velocity = movement;
    }

    #endregion

    #region Attacking

    private void StartShooting()
    {
        ChangeAnimationState(PLAYER_ATTACK);
        moveSpeed *= attackSlowDownModifier;
        StartCoroutine(ThrowAxe());

        Vector3 mousePos = GetMouseWorldPosition(Input.mousePosition) - this.transform.position;
        Flip(mousePos.x);
    }

    private IEnumerator ThrowAxe()
    {
        while (isAttacking)
        {
            yield return new WaitForSeconds(fireRate * 0.72f);
            Vector3 mousePos = GetMouseWorldPosition(Input.mousePosition) - this.transform.position;
            Flip(mousePos.x);

            Vector3 aimDirection = (mousePos).normalized;
            var aimAngle = Mathf.Atan2(aimDirection.y, aimDirection.x);
            float aimAngleDeg = aimAngle * Mathf.Rad2Deg;
            axeSpawnPoint.transform.eulerAngles = new Vector3(0, 0, aimAngleDeg);

            GameObject bullet = Instantiate(axePrefab, axeSpawnPoint.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();

            float xComponent = Mathf.Cos(aimAngle) * projectileForce;
            float zComponent = Mathf.Sin(aimAngle) * projectileForce;

            Vector2 forceApplied = new Vector2(xComponent, zComponent);
            rb.AddForce(forceApplied, ForceMode2D.Impulse);

            yield return new WaitForSeconds(fireRate * 0.28f);
        }

        AttackComplete();
    }

    public int DeliverDamage()
    {
        return damageDealt;
    }

    public void TakeDamage(int damage)
    {
        Debug.Log(currentHealth);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
        // if health == 0: Die();
    }

    #endregion

    void AttackComplete()
    {
        moveSpeed /= attackSlowDownModifier;
        isAttacking = false;
    }

    public void AwardExperience(int xp)
    {
        experience += xp;
        Debug.Log("Gained experience! Current value:" + experience);
        // check for lvl up
    }

    // utility method: get mouse world position
    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
