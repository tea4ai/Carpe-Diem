using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    [SerializeField] private Gradient gradient;
    [SerializeField] private Image fill;

    public void setMaxHealth(int health)
    {
        slider.maxValue = health;
        setHealth(health);
        fill.color = gradient.Evaluate(1f);
    }
    public void setHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}
