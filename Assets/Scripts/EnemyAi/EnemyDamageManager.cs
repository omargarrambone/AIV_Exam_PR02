using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    static private float damage, stunDamage;

    public HealthManager HealthManager;
    public StunnManager StunnManager;
    public ParticleSystem ucelletti;
    public bool PlayerIsAttacking;
    public float ParryTimeIndex;
    //public bool IsParrying;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            HealthManager.TakeDamage(damage);
            StunnManager.TakeStunn(stunDamage);            
            PlayerIsAttacking = true;
            ParryTimeIndex = Random.Range(0f, 1f);
            //if (ParryTimeIndex < 0.4f)
            //{
            //    IsParrying = true;
            //    ChangeDamage(0f, 0f);
            //}
            //else
            //{
            //    IsParrying = false;
            //    ChangeDamage();
            //}
        }
    }

    public static void ChangeDamage(float bloodDamage=10f, float stunnDamage=20f)
    {
        damage = bloodDamage;
        stunDamage = stunnDamage;
    }
   
    private void OnTriggerExit(Collider other)
    {
        PlayerIsAttacking = false;
    }

    public void SpawnParticles()
    {
        ParticleSystem go = Instantiate(ucelletti, transform);
        go.gameObject.SetActive(true);
        go.Play();
    }


}
