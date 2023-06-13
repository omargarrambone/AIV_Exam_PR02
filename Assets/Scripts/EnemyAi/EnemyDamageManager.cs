using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    public HealthManager HealthManager;
    public StunnManager StunnManager;
    public ParticleSystem ucelletti;
    public bool PlayerIsAttacking;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            float damage = 10;
            float stunnDamage = 20;
            HealthManager.TakeDamage(damage);
            StunnManager.TakeStunn(stunnDamage);
            PlayerIsAttacking = true; 
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerIsAttacking = false;
    }

    public void SpawnParticles()
    {
        ParticleSystem go = Instantiate(ucelletti, transform);
        go.Play();
    }

}
