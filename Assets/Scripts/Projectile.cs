using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag != "Player" && collision.gameObject.tag != "Axe")
        {
            StuckIn(collision);
            Destroy(gameObject, 2f);
        }

        if (collision.tag == "PickAxe")
        {
            Destroy(gameObject);
        }
    }

    void StuckIn(Collider2D collision)
    {
        gameObject.GetComponent<Animator>().Play("AxeStuck");
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
