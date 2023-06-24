using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthBarScript : MonoBehaviour
{
    public Slider slider;

    public Gradient gradient;

    public Image fill;

    public virtual void SetMaxHealth(float health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }

    public void SetMinStunnValue(float stunn)
    {
        slider.minValue = stunn;
        slider.value = stunn;

        fill.color = gradient.Evaluate(1f);
    }

    public virtual void SetHealth(float health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }

    public void SetStunn(float value)
    {
        slider.value = value;
        fill.color = gradient.Evaluate(value);
    }
}
