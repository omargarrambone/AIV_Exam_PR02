using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthBarScript : BarScript
{
    public Image border;

    public override void SetHealth(float health)
    {
        base.SetHealth(health);
        border.color = fill.color;
    }

    public override void SetMaxHealth(float health)
    {
        base.SetMaxHealth(health);
        border.color = fill.color;
    }
}
