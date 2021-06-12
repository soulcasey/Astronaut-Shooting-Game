using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public GameObject fill;

    void FixedUpdate()
    {
        Color bar = Color.red;
        fill.GetComponent<Image>().color = bar;
    }
    public void SetMaxHealth(float hp)
    {
        slider.maxValue = hp;
        slider.value = hp;
    }

    public void SetHealth(float hp)
    {
        slider.value = hp;
    }
}
