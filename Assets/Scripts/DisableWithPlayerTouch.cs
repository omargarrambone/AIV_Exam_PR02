using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableWithPlayerTouch : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
            enabled = false;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Player")
        {
            gameObject.SetActive(false);
            transform.parent.gameObject.SetActive(false);
            enabled = false;
        }
    }
}
