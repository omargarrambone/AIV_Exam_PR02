using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteScript : MonoBehaviour
{
    private AudioClip[] audioClips;
    [SerializeField] private AudioClip downClip, rightClip, upClip, leftClip, wrongClip;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float radius,maxDistance;
    [SerializeField] private LayerMask enemiesLayer;
    [SerializeField] bool isAttacking;
    [SerializeField] float attackDmg;
    [SerializeField] FluteUIScript fluteUIScript;
    public ParticleSystem Dissolve;

    private void Start()
    {
        audioClips = new AudioClip[((int)FluteArrow.LAST)];
        audioClips[0] = downClip;
        audioClips[1] = rightClip;
        audioClips[2] = upClip;
        audioClips[3] = leftClip;
    }

    public void ActivateMinigame(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed)
        {
            Collider[] hittedEnemies = GetHittedEnemies();
            bool atLeastOneEnemyStunned = false;

            if (hittedEnemies.Length > 0)
            {
                foreach (Collider enemy in hittedEnemies)
                {
                    GameObject enemyObj = enemy.gameObject;
                    StunnManager stunnMngr = enemyObj.GetComponent<StunnManager>();

                    if (stunnMngr.IsStunned)
                    {
                        atLeastOneEnemyStunned = true;
                    }
                }
            }

            if (atLeastOneEnemyStunned)
            {
                fluteUIScript.gameObject.SetActive(true);
            }
        }
    }

    public Collider[] GetHittedEnemies()
    {
        return Physics.OverlapSphere(transform.position, radius, enemiesLayer);
    }

    [ContextMenu("PlaySound")]
    public void PlaySound()
    {
        Collider[] hittedEnemies = GetHittedEnemies();

        audioSource.Stop();
        audioSource.Play();

        if (hittedEnemies.Length > 0)
        {
            //purify attack
            foreach (Collider enemy in hittedEnemies)
            {
                GameObject enemyObj = enemy.gameObject;
                StunnManager stunnMngr = enemyObj.GetComponent<StunnManager>();

                if (stunnMngr.IsStunned)
                {
                    ParticleSystem dis = Instantiate(Dissolve, enemyObj.transform.position, Dissolve.transform.rotation);                    
                    Destroy(enemyObj, 0.2f);
                    Destroy(dis.gameObject, 1);
                    if (!stunnMngr.NotCountsForStunCount)
                    {
                        PowerUpManager.SpawnPowerUpRandom(enemyObj.transform.position);
                        NPCCounter.AddPurifiedNPCToCounter();
                    }
                }
            }

        }
        else
        {
            //timer attack
            isAttacking = true;
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

    private void Update()
    {
        if (isAttacking)
        {
            if (audioSource.isPlaying)
            {
                Collider[] hittedEnemies = GetHittedEnemies();

                foreach (Collider enemy in hittedEnemies)
                {
                    enemy.GetComponent<HealthManager>().AddHealth(-attackDmg*Time.deltaTime);
                    enemy.gameObject.SetActive(false);
                }
            }
            else
            {
                isAttacking = false;
            }
        }
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
