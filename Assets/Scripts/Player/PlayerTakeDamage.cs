using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.VFX;

public class PlayerTakeDamage : MonoBehaviour
{
    public HealthManager HealthManager;
    public VisualEffect bloodFx;
    public Material invincibleMaterial, defaultMaterial;
    public SkinnedMeshRenderer meshRenderer;

    private float delay = 0.61f;
    private float timer = 0;
    private bool enemyIsAttacking = false;

   [SerializeField] private float invincibilityTimer = 1f;
   [SerializeField] private float invincibilityCounter;

    private void OnTriggerEnter(Collider other)
    {
        enemyIsAttacking = true;

        if (other.gameObject.tag == "EnemyWeapon")
        {
            bloodFx.gameObject.SetActive(true);
            bloodFx.playRate = 1f;
            bloodFx.Play();
            HealthManager.TakeDamage(other.GetComponent<EnemyWeapon>().MyWeaponDamage);

            invincibilityCounter = invincibilityTimer;
            HealthManager.IsImmune = true;

            meshRenderer.material = invincibleMaterial;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        enemyIsAttacking = false;   
    }

    private void Update()
    {
        if (enemyIsAttacking)
        {
            if (timer < delay)
            {
                timer += Time.deltaTime;
            }
            if (timer >= delay)
            {
                bloodFx.gameObject.SetActive(false);
                timer = 0;
            }
        }

        if (HealthManager.IsImmune)
        {
            invincibilityCounter -= Time.deltaTime;

            if (invincibilityCounter < 0)
            {
                meshRenderer.material = defaultMaterial;
                HealthManager.IsImmune = false;
            }
        }
    }



}
