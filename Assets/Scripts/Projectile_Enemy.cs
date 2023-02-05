using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile_Enemy : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Enemy" && collision.gameObject.tag != "PickAxe")
        {
            StuckIn(collision);
            Destroy(gameObject, 2f);
        }

        if (collision.tag == "Axe")
        {
            Destroy(gameObject);
        }
    }

    void StuckIn(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().Play("PickAxeStuck");
        var velocity = gameObject.GetComponent<Rigidbody2D>().velocity;
        if (velocity.x < 0 && velocity.y < 0)
        {
            Vector3 theScale = transform.localScale;
            theScale.x *= -1;
            transform.localScale = theScale;
        }
        gameObject.GetComponent<Rigidbody2D>().velocity = Vector3.zero;
        transform.parent = collision.transform;
    }
}
