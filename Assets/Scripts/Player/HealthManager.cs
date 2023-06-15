using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;
using UnityEditor;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class HealthManager : MonoBehaviour
{
    private float MaxHealth = 100;
    public float CurrentHealth;

    public HealthBarScript HealthBar;
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        if (CurrentHealth <= 0)
        {
            GetComponent<CapsuleCollider>().enabled = false;
            GetComponent<Animator>().enabled = false;
        }
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
