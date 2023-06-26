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
    public float ParryTimeIndex;
    public bool IsParrying;
    public bool IsHitting;
    public float ParryChance = 0.4f;
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
            //HealthManager.TakeDamage(damage);
            //StunnManager.TakeStunn(stunDamage);

            PlayerIsAttacking = true;
            ParryTimeIndex = Random.Range(0f, 1f);
            if (ParryTimeIndex < ParryChance && StunnManager.IsStunned == false)
            {
                IsParrying = true;
                HealthManager.TakeDamage(0f);
                StunnManager.TakeStunn(0f);
            }           
            else 
            {
                IsParrying = false;
                //IsHitting = false;
                HealthManager.TakeDamage(damage);
                StunnManager.TakeStunn(stunDamage);
            }

            counter = timer;
        }
        
        if (other.gameObject.tag == "Kick")
        //if (ParryTimeIndex < HitChance && StunnManager.IsStunned == false)
        {
            IsHitting = true;
            GetComponent<Animator>().Play("Knockback");
            //HealthManager.TakeDamage(0f);
            //StunnManager.TakeStunn(0f);
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
