using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth;
    public float CurrentStunn;
    public float MinStunnValue = 0;

    public HealthBarScript HealthBar;
    public HealthBarScript StunnBar;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
        StunnBar.SetMinStunnValue(MinStunnValue);
    }

    public void AddHealth(float health)
    {
        CurrentHealth += health;
        HealthBar.SetHealth(CurrentHealth);
    }

    public void TakeDamage(float damage, float stunnDamage = 0)
    {
        CurrentHealth -= damage;
        CurrentStunn += stunnDamage;

        HealthBar.SetHealth(CurrentHealth);
        StunnBar.SetStunn(CurrentStunn);
    }
}
