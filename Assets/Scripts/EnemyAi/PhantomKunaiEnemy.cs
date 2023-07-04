using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhantomKunaiEnemy : KunaiEnemy
{

    // Update is called once per frame
    protected override void Update()
    {
        MovingKunaiEnemy();
    }

    protected override void MovingKunaiEnemy()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;

        switch (currentState)
        {
            case EnemyState.Chase:

                if (distanceFromTarget.magnitude <= attackDistance)
                {
                    agent.speed = 0;
                    currentState = EnemyState.Attack;
                    break;
                }

                agent.speed = chaseSpeed;
                agent.SetDestination(playerTarget.position);
                break;

            case EnemyState.Attack:

                anim.SetBool("Attack", true);
                agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0, distanceFromTarget.normalized.z);

                if (enemyDamageManager.IsParrying)
                {
                    anim.SetBool("IsParrying", true);
                    parryTimer -= Time.deltaTime;

                    if (parryTimer <= 0)
                    {
                        enemyDamageManager.IsParrying = false;
                        anim.SetBool("IsParrying", false);
                        parryTimer = 0.3f;
                    }
                    break;
                }

                if (enemyDamageManager.IsHitting && stunnManager.IsStunned == false)
                {
                    enemyDamageManager.IsHitting = false;
                    break;
                }

                if (distanceFromTarget.magnitude > attackDistance)
                {
                    anim.SetBool("Attack", false);
                    agent.speed = chaseSpeed;
                    currentState = EnemyState.Chase;
                    break;
                }

                break;

            case EnemyState.Dead:
                
                this.enabled = false;
                anim.enabled = false;
                enemyCollider.enabled = false;
                healthBar.SetActive(false);
                stunBar.SetActive(false);
                arancini.gameObject.SetActive(false);
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:
                if (healthManager.IsDead)
                {
                    currentState = EnemyState.Dead;
                    break;
                }
                anim.SetBool("Stunned", true);
                arancini.gameObject.SetActive(true);
                anim.SetBool("Attack", false);
                agent.isStopped = true;

                if (stunnManager.CurrentStunn < 1)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Chase;
                    arancini.gameObject.SetActive(false);
                    stunnManager.IsStunned = false;
                    agent.isStopped = false;
                }
                break;
        }
    }
}
