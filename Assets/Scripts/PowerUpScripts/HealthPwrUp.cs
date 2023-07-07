using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "CustomMenu/PowerUP/Health")] 

public class HealthPwrUp : PowerUpScriptable
{
    public int healthAmount;
    public override void Apply(GameObject target)
    {
        target.GetComponent<HealthManager>().AddHealth(healthAmount);
    }
}
