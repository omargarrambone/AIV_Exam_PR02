using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpManager powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.CompareTag("Player"))
        {
            Destroy(gameObject);
            powerUpEffect.Apply(other.gameObject);
        }
    }
}
