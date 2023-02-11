using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelFinish : MonoBehaviour
{
    [SerializeField] GameObject doorOpen;
    [SerializeField] GameObject doorClosed;

    public void Awake()
    {
        doorOpen.SetActive(true);
        doorClosed.SetActive(false);
    }
}
