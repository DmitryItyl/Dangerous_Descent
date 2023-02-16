using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    // Speed and modifiers
    [SerializeField] float moveSpeed;
    private float attackSlowDownModifier = 0.2f;
    private float xAxis;
    private float yAxis;

    // Shooting
    [SerializeField] GameObject axePrefab;
    [SerializeField] float projectileForce;
    [SerializeField] float fireRate = 0.68f;
    [SerializeField] int damageDealt;

    // Health and stats
    [SerializeField] int maxHealth;
    private int currentHealth;
    private int experience;
    private int levelUPsAvailable = 0;

    // Objects references
    private Animator animator;
    private Rigidbody2D rb;
    private Transform axeSpawnPoint;

    // General states
    private bool isAlive;
    private bool isFacingRight = true;
    private bool isAttackPressed;
    private bool isAttacking;
    private bool deatAnimOver = false;

    // Animation states
    const string PLAYER_ATTACK = "Attack";
    const string PLAYER_RUN = "Run";
    const string PLAYER_IDLE = "Idle";
    const string PLAYER_DEATH = "Death";
    private string currentState;

    // UI references
    HPBar hpBar;
    MenuController levels;
    GameObject levelUpButton;
    GameObject gameOverScreen;

    // Progression
    bool isStageClear;
    List<string> powerUps = new List<string>();

    private void Awake()
    {
        // new stage is always not cleared
        isStageClear = false;
    }

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        axeSpawnPoint = transform.Find("AxeSpawnPoint");
        currentHealth = maxHealth;

        isAlive = true;

        hpBar = GameObject.Find("HPBar").GetComponentInChildren<HPBar>();
        levels = GameObject.Find("HUD").GetComponent<MenuController>();
        levelUpButton = GameObject.Find("LevelUpButton");
        levelUpButton.SetActive(false);

        gameOverScreen = GameObject.Find("GameOverScreen");
        gameOverScreen.SetActive(false);
    }

    void Update()
    {
        CheckInput();
    }

    private void FixedUpdate()
    {
        if (!isAlive)
            return;

        Move(xAxis, yAxis);
        HandleAttack();
    }

    void CheckInput()
    {
        xAxis = Input.GetAxisRaw("Horizontal");
        yAxis = Input.GetAxisRaw("Vertical");

        isAttackPressed = Input.GetButton("Fire1");
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
        Debug.Log(moveSpeed);

        var movement = new Vector2(horizontal, vertical).normalized * moveSpeed;

        if (currentState != "Attack")
            Flip(horizontal);

        rb.velocity = movement;

        if (!isAttacking && !isAttackPressed)
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

    #endregion

    #region Attacking

    void HandleAttack()
    {
        if (isAttackPressed)
        {
            if (!isAttacking)
            {
                isAttacking = true;
                StartShooting();
            }
        }
    }

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

        AttackComplete();
    }

    void AttackComplete()
    {
        moveSpeed /= attackSlowDownModifier;
        isAttacking = false;
    }

    public int DeliverDamage()
    {
        return damageDealt;
    }

    public void CheckClear()
    {
        var enemies = GameObject.FindGameObjectsWithTag("Enemy").ToList();
        if (enemies.Count == 0)
        {
            Debug.Log("All clear");
            isStageClear = true;
            levels.CurrentLevelClear();
        }
    }

    public void TakeDamage(int damage, bool healInsted = false)
    {
        if (healInsted)
            damage = -damage;
        currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);

        hpBar.SetHealthPencents((float)currentHealth / (float)maxHealth);

        if (currentHealth <= 0)
            GameOver();
    }

    #endregion


    public void AwardExperience(int xp)
    {
        experience += xp;
        Debug.Log("Gained experience! Current value:" + experience);

        if (experience >= 100)
        {
            levelUpButton.SetActive(true);
            levelUPsAvailable++;
            experience -= 100;
        }
    }

    public void ApplyPowerUp(string powerUpName)
    {
        switch (powerUpName)
        {
            case "Heal":
                TakeDamage(40, true);
                break;
            case "Damage":
                damageDealt += 5;
                powerUps.Add(powerUpName);
                break;
            case "Speed":
                moveSpeed *= 1.2f;
                powerUps.Add(powerUpName);
                break;
        }
    }

    // utility method: get mouse world position
    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }

    void GameOver()
    {
        //Time.timeScale = 0f;
        isAlive = false;
        StartCoroutine(PlayDeathAnimation());

        if (deatAnimOver)
        {
            Time.timeScale = 0f;

            gameOverScreen.SetActive(true);
            var gameOverCanvas = gameOverScreen.GetComponent<Canvas>().GetComponent<CanvasGroup>();
            DoFadeIn(gameOverCanvas);
        }
    }

    IEnumerator DoFadeIn(CanvasGroup canvas)
    {
        while (canvas.alpha < 1)
        {
            canvas.alpha += 0.01f;
            yield return null;
        }
    }

    IEnumerator PlayDeathAnimation()
    {
        animator.Play(PLAYER_DEATH);
        yield return new WaitForSeconds(0.8f);
        deatAnimOver = true;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Finish")
            levels.LoadNextLevel();            
    }
}
