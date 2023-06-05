using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class HealthManager : MonoBehaviour
{
    public int MaxHealth = 100;
    public int CurrentHealth;

    public HealthBarScript HealthBar;


    // Start is called before the first frame update
    void Start()
    {
        CurrentHealth = MaxHealth;
        HealthBar.SetMaxHealth(MaxHealth);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AddHealth(int health)
    {
        CurrentHealth += health;
        HealthBar.SetHealth(CurrentHealth);
    }

    public void TakeDamage(int damage)
    {
        CurrentHealth -= damage;

        HealthBar.SetHealth(CurrentHealth);
    }
}
