using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;
using UnityEditor;



public class HealthManager : MonoBehaviour
{
    public float MaxHealth {get; private set;}
    public float CurrentHealth;
    public UnityEvent OnDeath;
    private Animator anim;

    public bool IsDead { get { return CurrentHealth <= 0; } }

    public HealthBarScript HealthBar;
    void Start()
    {
        MaxHealth = 100;

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
        if (IsDead)
        {
            return;
        }

        CurrentHealth -= damage;

        if (IsDead)
        {
            //GetComponent<CapsuleCollider>().enabled = false;
            anim = GetComponent<Animator>();
            anim.SetTrigger("Death");
        }

        HealthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
