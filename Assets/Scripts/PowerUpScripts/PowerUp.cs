using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public PowerUpScriptable powerUpEffect;
    private float yOriginal;
    private float ySpeed = 2.5f;
    private float cosAmplitude = 0.5f;

    private void Start()
    {
        yOriginal = transform.localPosition.y;
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<HealthManager>().CurrentHealth < 100)
        {
            gameObject.SetActive(false);
            powerUpEffect.Apply(other.gameObject);
        }
    }

    private void Update()
    {
        transform.localPosition = new Vector3(transform.localPosition.x, yOriginal + Mathf.Cos(Time.time * ySpeed) * cosAmplitude, transform.localPosition.z);

        transform.Rotate(Vector3.forward * Time.deltaTime,1f);
    }
}
