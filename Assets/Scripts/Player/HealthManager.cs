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
    private float MaxHealth = 100;
    public float CurrentHealth;
    public UnityEvent OnDeath;
    private Animator _anim;

    public bool IsDead { get { return CurrentHealth <= 0; } }

    public HealthBarScript HealthBar;
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    private void Update()
    {
        
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
            _anim = GetComponent<Animator>();
            _anim.SetTrigger("Death");
        }

        HealthBar.SetHealth(CurrentHealth);

        if (CurrentHealth <= 0)
        {
            OnDeath.Invoke();
        }
    }
}
