using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "PowerUP/Health")] 

public class HealthPwrUp : PowerUpManager
{
    public float healthAmount;
    public override void Apply(GameObject target)
    {
        //target.GetComponent<Health>().health.value += healthAmount;
    }
}
