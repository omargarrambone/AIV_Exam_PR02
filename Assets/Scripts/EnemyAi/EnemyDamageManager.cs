using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    static private float damage, stunDamage;

    public HealthManager HealthManager;
    public StunnManager StunnManager;
    public ParticleSystem arancini;
    public bool PlayerIsAttacking;

    public float timer, counter;

    private void Start()
    {
        timer = 1f;
    }

    private void Update()
    {
        if (PlayerIsAttacking)
        {
            counter -= Time.deltaTime;

            if (counter < 0)
            {
                PlayerIsAttacking = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            HealthManager.TakeDamage(damage);
            StunnManager.TakeStunn(stunDamage);

            PlayerIsAttacking = true;
            counter = timer;
        }
    }

    public static void ChangeDamage(float bloodDamage=10f, float stunnDamage=20f)
    {
        damage = bloodDamage;
        stunDamage = stunnDamage;
    }

    public void SpawnParticles()
    {
        ParticleSystem go = Instantiate(arancini, transform);
        go.gameObject.SetActive(true);
        go.Play();
    }


}
