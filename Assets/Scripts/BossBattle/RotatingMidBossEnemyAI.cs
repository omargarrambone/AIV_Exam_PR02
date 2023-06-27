using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RotatingMidBossEnemyAI : BasicEnemyAgentAi
{
    float dizzinessCounter, dizzinessTimer;

    protected override void Start()
    {
        base.Start();
        Weapon.GetComponent<BoxCollider>().enabled = true;
        dizzinessTimer = 4f;
    }

    protected override void Update()
    {
        Anim.SetFloat("Speed", Agent.velocity.magnitude);

        switch (CurrentState)
        {
            case EnemyState.Patrol:
                if (Agent.remainingDistance < 2f)
                {

                    Fov.Angle = 150;
                    SetNewWaypoint();
                    Agent.speed = PatrolSpeed;

                    dizzinessCounter += Time.deltaTime;

                    if(dizzinessCounter > dizzinessTimer)
                    {
                        CurrentState = EnemyState.Stun;
                    }
                }

                break;
            case EnemyState.Attack:

                Anim.SetBool("Attack", true);

                if (EnemyDamageManager.IsHitting && StunnManager.IsStunned == false)
                {
                    Anim.SetTrigger("IsHitting");

                    EnemyDamageManager.IsHitting = false;
                }

                break;

            case EnemyState.Dead:
                Agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                Agent.GetComponent<CapsuleCollider>().enabled = false;
                Weapon.GetComponent<BoxCollider>().enabled = false;
                Agent.GetComponent<Animator>().enabled = false;
                gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                Arancini.gameObject.SetActive(false);
                SpawnPowerUp(HeavyHealth);
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:
                if (HealthManager.IsDead)
                {
                    CurrentState = EnemyState.Dead;
                    break;
                }
                Anim.SetBool("Stunned", true);
                Arancini.gameObject.SetActive(true);
                Weapon.GetComponent<BoxCollider>().enabled = false;
                IsAttacking = false;
                Anim.SetBool("Attack", false);
                Agent.speed = 0;

                //Enemy ready to be purified by the sound of the Magic Flute

                if (StunnManager.CurrentStunn < 1)
                {
                    Anim.SetBool("Stunned", false);
                    CurrentState = EnemyState.Patrol;
                    //Agent.speed = PatrolSpeed;
                    Arancini.gameObject.SetActive(false);
                    StunnManager.IsStunned = false;
                }

                break;
        }
    }
}

