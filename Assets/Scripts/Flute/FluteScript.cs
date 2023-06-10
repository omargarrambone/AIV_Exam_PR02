using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteScript : MonoBehaviour
{
    private AudioClip[] audioClips;
    [SerializeField] private AudioClip downClip, rightClip, upClip, leftClip, wrongClip;
    private AudioSource audioSource;
    [SerializeField] private float radius,maxDistance;
    [SerializeField] private LayerMask enemiesLayer;

    [SerializeField] bool isAttacking;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioClips = new AudioClip[((int)FluteArrow.LAST)];
        audioClips[0] = downClip;
        audioClips[1] = rightClip;
        audioClips[2] = upClip;
        audioClips[3] = leftClip;
    }

    public RaycastHit[] GetHittedEnemies()
    {
        return Physics.SphereCastAll(transform.position, radius, transform.forward, maxDistance, enemiesLayer);
    }

    [ContextMenu("PlaySound")]
    public void PlaySound()
    {
        RaycastHit[] hittedEnemies = GetHittedEnemies();

        audioSource.Stop();
        audioSource.Play();

        if (hittedEnemies.Length > 0)
        {
            //purify attack


            foreach (RaycastHit enemy in hittedEnemies)
            {
                GameObject enemyObj = enemy.collider.gameObject;
                EnemyPurification enemyPurification = enemyObj.GetComponent<EnemyPurification>();

                //TODO: check se il nemico è stordito

                if (enemyPurification is not null)
                {
                    if (enemyPurification.IsStunned)
                        enemyObj.transform.SetLocalPositionAndRotation(enemyPurification.PurificatedLocation.position, enemyPurification.PurificatedLocation.rotation);
                }
            }

        }
        else
        {
            //timer attack

            StartCoroutine(AreaAttack());

        }

    }

    public void PlayCorrectNote(FluteArrow fluteArrow)
    {
        audioSource.PlayOneShot(audioClips[(int)fluteArrow]);
    }

    public void PlayWrongNote()
    {
        audioSource.PlayOneShot(wrongClip);
    }

    IEnumerator AreaAttack()
    {
        while (audioSource.isPlaying)
        {
            RaycastHit[] hittedEnemies = GetHittedEnemies();

            foreach (RaycastHit enemy in hittedEnemies)
            {
                Destroy(enemy.collider.gameObject);
            }

            isAttacking = true;

            Debug.Log("INIZIATA INEMURATOR " + Time.deltaTime);
        }

        yield return audioSource.isPlaying;
        isAttacking = false;
        Debug.Log("INIZIATA INEMURATOR "+Time.deltaTime);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (isAttacking)
        {
            Gizmos.color = Color.green;
        }
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
