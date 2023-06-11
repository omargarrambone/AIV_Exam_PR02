using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
using UnityEngine.Events;

public class HealthManager : MonoBehaviour
{
    public float MaxHealth = 100;
    public float CurrentHealth;
    public float CurrentStunn;
    public float MinStunnValue = 0;
    public bool IsStunned;

    public HealthBarScript HealthBar;
    public HealthBarScript StunnBar;

    public UnityEvent OnStun;

    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        CurrentStunn = MinStunnValue;
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
        if (IsStunned) return;

        CurrentHealth -= damage;
        CurrentStunn += stunnDamage;

        HealthBar.SetHealth(CurrentHealth);
        StunnBar.SetStunn(CurrentStunn);

        if (CurrentStunn >= 100)
        {
            OnStun.Invoke();
            IsStunned = true;
        }
    }
}
