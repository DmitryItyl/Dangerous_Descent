using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrystalController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var crystals = GameObject.FindGameObjectsWithTag("Crystal");
        Debug.Log(crystals.Length);
        foreach (GameObject crystal in crystals)
        {
            Animator animator = crystal.GetComponentInChildren<Animator>();
            int animation_number = UnityEngine.Random.Range(1, 3);
            float delay = UnityEngine.Random.Range(0.0f, 6.0f);
            StartCoroutine(StartGlowing(animator, animation_number, delay)); 
        }

    }

    IEnumerator StartGlowing(Animator animator, int animation_number, float delay)
    {
        Debug.Log("New crystal animation: " + animation_number + " " + delay);
        yield return new WaitForSeconds(delay);
        animator.Play("CrystalFlicker_" + animation_number);
    }
}
