using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    public HealthManager HealthManager;
    public ParticleSystem ucelletti;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            float damage = 10;
            float stunnDamage = 20;
            HealthManager.TakeDamage(damage,stunnDamage);
        }
    }

    public void SpawnParticles()
    {
        ParticleSystem go = Instantiate(ucelletti,transform);
        go.Play();
    }

}
