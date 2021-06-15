using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RobotHealthBar : MonoBehaviour
{
    public Slider slider;
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
