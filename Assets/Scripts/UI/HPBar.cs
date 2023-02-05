using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    List<GameObject> elements;
    float oneChunkValue;
    float currentHealthPencents;
    int activeChunks;

    void Start()
    {
        elements = new List<GameObject>();
        foreach (Transform child in transform)
        {
            elements.Add(child.gameObject);
        }

        oneChunkValue = 1.0f / elements.Count;
        activeChunks = elements.Count;
        currentHealthPencents = 1.0f;
    }

    public void SetHealthPencents(float health)
    {
        currentHealthPencents = health;
        activeChunks = Mathf.CeilToInt(currentHealthPencents / oneChunkValue);

        RefreshChunksVisibility();
    }

    public void RefreshChunksVisibility()
    {
        for (int i = 0; i < elements.Count; i++)
        {
            elements[i].SetActive(i <= activeChunks);
        }
    }

    public int TrimToNElements(int n)
    {
        if (elements.Count() != n)
            elements = elements.Take(n).ToList();

        return elements.Count();
    }
}
