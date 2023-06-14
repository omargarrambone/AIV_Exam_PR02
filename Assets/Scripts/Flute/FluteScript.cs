using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluteScript : MonoBehaviour
{
    public int PurifiedEnemies;
    public int KilledEnemies;

    private AudioClip[] audioClips;
    [SerializeField] private AudioClip downClip, rightClip, upClip, leftClip, wrongClip;
    private AudioSource audioSource;
    [SerializeField] private float radius,maxDistance;
    [SerializeField] private LayerMask enemiesLayer;

    [SerializeField] bool isAttacking;
    [SerializeField] float attackDmg;
    [SerializeField] FluteUIScript fluteUIScript;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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
            RaycastHit[] hittedEnemies = GetHittedEnemies();
            bool atLeastOneEnemyStunned = false;

            if (hittedEnemies.Length > 0)
            {
                foreach (RaycastHit enemy in hittedEnemies)
                {
                    GameObject enemyObj = enemy.collider.gameObject;
                    StunnManager stunnMngr = enemyObj.GetComponent<StunnManager>();

                    if (stunnMngr.IsStunned)
                    {
                        atLeastOneEnemyStunned = true;
                    }
                }
            }

            if(atLeastOneEnemyStunned)
                fluteUIScript.gameObject.SetActive(true);
        }
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
                StunnManager stunnMngr = enemyObj.GetComponent<StunnManager>();


                if (stunnMngr.IsStunned)
                {
                    Destroy(enemyObj);
                    PurifiedEnemies++;
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
                RaycastHit[] hittedEnemies = GetHittedEnemies();

                foreach (RaycastHit enemy in hittedEnemies)
                {
                    //TODO: effettuare danno al nemico con un float
                    //enemy.collider.GetComponent<HealthManager>().AddHealth(-attackDmg*Time.deltaTime);
                    enemy.collider.gameObject.SetActive(false);
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
