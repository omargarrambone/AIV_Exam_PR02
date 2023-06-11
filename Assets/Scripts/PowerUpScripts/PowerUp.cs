using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpManager powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<HealthManager>().CurrentHealth < 100)
        {
            gameObject.SetActive(false);
            powerUpEffect.Apply(other.gameObject);
        }
    }
}
