using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteScript : MonoBehaviour
{
    private AudioSource audioSource;
    [SerializeField] private float radius,maxDistance;
    [SerializeField] private LayerMask enemiesLayer;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    [ContextMenu("PlaySound")]
    public void PlaySound()
    {
        audioSource.Play();

        RaycastHit[] hittedObjects = Physics.SphereCastAll(transform.position, radius, transform.forward, maxDistance, enemiesLayer);

        foreach (RaycastHit enemy in hittedObjects)
        {
            //spawn ucelletti in testa a nemici
            //GameObject birds = Instantiate(fogSystemPrefab, enemy.collider.transform);
            //birds.GetComponent<ParticleSystem>().Play();

            // nemico nell'hub
            GameObject enemyObj = enemy.collider.gameObject;
            EnemyPurification enemyPurification = enemyObj.GetComponent<EnemyPurification>();

            if (enemyPurification is not null)
            {
                if (enemyPurification.IsStunned)
                    enemyObj.transform.SetLocalPositionAndRotation(enemyPurification.PurificatedLocation.position, enemyPurification.PurificatedLocation.rotation);
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
