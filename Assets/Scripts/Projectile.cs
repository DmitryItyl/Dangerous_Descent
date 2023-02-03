using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(AutoDestroy(2.0f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private IEnumerator AutoDestroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(gameObject);
    }
}
