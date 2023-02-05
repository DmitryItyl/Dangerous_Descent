using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarController : MonoBehaviour
{
    [SerializeField] Slider Slider;
    [SerializeField] Color Low;
    [SerializeField] Color High;
    [SerializeField] Vector3 offset;

    bool isActive;

    public void SetHealth(float health, float maxHealth)
    {
        Slider.gameObject.SetActive(health < maxHealth);
        Slider.value = health;
        Slider.maxValue = maxHealth;


        Slider.fillRect.GetComponentInChildren<Image>().color = Color.Lerp(Low, High, Slider.normalizedValue);
    }

    public void TurnOff()
    {
        Destroy(Slider.gameObject);
        isActive = false;
    }

    void Update()
    {
        if(isActive)
            Slider.transform.position = Camera.main.WorldToScreenPoint(transform.parent.position + offset);

    }
}
