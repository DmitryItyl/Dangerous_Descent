using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotroller : MonoBehaviour
{
    [SerializeField] float moveSpeed;

    [SerializeField] GameObject axePrefab;
    [SerializeField] float projectileForce;

    public float fireRate = 0.4f;

    Vector2 movement;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;

    private Transform axeSpawnPoint;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        axeSpawnPoint = transform.Find("AxeSpawnPoint");
    }

    private void Start()
    {
        StartCoroutine(ShootingRateCoroutine());
    }

    void Update()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

        /* if (Input.GetButtonDown("Fire1"))
         {
             StartShooting();
         }*/


    }

    private void FixedUpdate()
    {
        rb.velocity = movement;
    }

    private void Flip(bool playerLooksRight)
    {
        isFacingRight = playerLooksRight;
        Vector3 theScale = transform.localScale;

        if (!playerLooksRight)
            theScale.x = -Mathf.Abs(theScale.x);
        else
            theScale.x = Mathf.Abs(theScale.x);

        transform.localScale = theScale;
    }

    private void Move(float horizontal, float vertical)
    {
        if (horizontal > 0 && !isFacingRight)
        {
            Flip(true);
        }
        if (horizontal < 0 && isFacingRight)
        {
            Flip(false);
        };

        movement = new Vector2(horizontal, vertical).normalized * moveSpeed;

        if (movement.magnitude > 0)
            animator.SetBool("IsMoving", true);
        else
            animator.SetBool("IsMoving", false);
    }

    private void StartShooting()
    {
        animator.SetTrigger("IsAttacking");
        StartCoroutine(ThrowAxe());

    }

    private IEnumerator ThrowAxe()
    {
        yield return new WaitForSeconds(0.29f);

        Vector3 mousePos = GetMouseWorldPosition(Input.mousePosition) - this.transform.position;

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
    }

    IEnumerator ShootingRateCoroutine()
    {
        while (true)  //while (player.isActiveAndEnabled)
        {
            if (Input.GetButton("Fire1"))
            {
                StartShooting();
                yield return new WaitForSeconds(fireRate);
            }
            else
                yield return new WaitForSeconds(0.04f);
        }

    }


    // utility method: get mouse world position
    private Vector3 GetMouseWorldPosition(Vector3 screenPosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(screenPosition);
        worldPosition.z = 0f;
        return worldPosition;
    }
}
