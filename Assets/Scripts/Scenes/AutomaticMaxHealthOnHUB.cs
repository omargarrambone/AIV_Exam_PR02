using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutomaticMaxHealthOnHUB : MonoBehaviour
{
    void Start()
    {
        SetPlayerHealthToMaxHealth();
    }

    public void SetPlayerHealthToMaxHealth()
    {
        HealthManager hm = PlayerManager.PlayerGameObject.GetComponent<HealthManager>();

        if(hm != null)
        {
            hm.CurrentHealth = hm.MaxHealth;
        }
    }
}
