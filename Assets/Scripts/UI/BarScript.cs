using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class BarScript : MonoBehaviour
{
    public Gradient gradient;
    public Image fill;
    private float maxHealth;
    private float maxStun;

    public virtual void SetMaxHealth(float health)
    {
        maxHealth = health;
        fill.fillAmount = maxHealth;
        fill.color = gradient.Evaluate(1f);
    }

    public void SetMinStunnValue(float stun)
    {
        fill.fillAmount = stun;
        maxStun = 100;
        fill.color = gradient.Evaluate(1f);
    }

    public virtual void SetHealth(float health)
    {
        fill.fillAmount = health/maxHealth;
        fill.color = gradient.Evaluate(health / maxHealth);
    }

    public void SetStunn(float value)
    {
        fill.fillAmount = value/maxStun;
        fill.color = gradient.Evaluate(value / maxStun);
    }
}
