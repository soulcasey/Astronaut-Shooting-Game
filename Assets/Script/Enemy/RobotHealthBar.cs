using System;
using UnityEngine;
using UnityEngine.UI;

public class RobotHealthBar : MonoBehaviour
{
    public Slider slider;
    public RobotMovement robotMovement;

    private void Start()
    {
        slider.maxValue = RobotMovement.MAX_HEALTH;
    }

    private void Update()
    {
        slider.value = Math.Max(0, robotMovement.CurrentHealth);
    }
}
