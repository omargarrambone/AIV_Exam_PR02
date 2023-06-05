using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpManager powerUpEffect;

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            powerUpEffect.Apply(other.gameObject);
        }
    }
}
