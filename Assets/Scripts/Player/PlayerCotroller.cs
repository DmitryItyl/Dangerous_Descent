using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCotroller : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1.5f;

    Vector2 movement;

    private Animator animator;
    private Rigidbody2D rb;
    private bool isFacingRight = true;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        Move(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));

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
}
