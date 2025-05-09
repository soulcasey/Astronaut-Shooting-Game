using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;
    public PlayerMovement playerMovement;

    private void Start()
    {
        slider.maxValue = PlayerMovement.MAX_HEALTH;
    }

    private void Update()
    {
        slider.value = Math.Max(0, playerMovement.CurrentHealth);
    }
}
