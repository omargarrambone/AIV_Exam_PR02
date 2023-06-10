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

    public HealthBarScript HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    public void AddHealth(float health)
    {
        CurrentHealth += health;
        HealthBar.SetHealth(CurrentHealth);
    }

    public void TakeDamage(float damage)
    {
        CurrentHealth -= damage;

        HealthBar.SetHealth(CurrentHealth);
    }
}
