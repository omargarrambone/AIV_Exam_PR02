using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDamageManager : MonoBehaviour
{
    public HealthManager HealthManager;
    public ParticleSystem ucelletti;

    // Start is called before the first frame update
    void Update()
    {
        MaxStunn();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            float damage = 10;
            float stunnDamage = 20;
            HealthManager.TakeDamage(damage,stunnDamage);
        }
    }

    void MaxStunn()
    {
        if (HealthManager.CurrentStunn >= 100)
        {
            ucelletti.gameObject.SetActive(true);
            ucelletti.Play();
        }
    }

}
