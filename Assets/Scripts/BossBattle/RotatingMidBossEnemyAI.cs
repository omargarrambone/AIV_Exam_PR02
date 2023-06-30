using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMidBossEnemyAI : BasicEnemyAgentAi
{
    [SerializeField] private float dizzinessCounter, dizzinessTimer;

    protected override void Start()
    {
        base.Start();
        weapon.GetComponent<BoxCollider>().enabled = true;
        //dizzinessTimer = 4f; // random float 4 - 7

        stunnManager.StunnDecreaseVelocity = 50.0f;
        stunnManager.Timer = 2f;

    }

    protected override void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:

                dizzinessCounter += Time.deltaTime;

                if (dizzinessCounter > dizzinessTimer)
                {
                    dizzinessCounter = 0;
                    currentState = EnemyState.Dizzy;
                    break;
                }

                if (agent.remainingDistance < 2f)
                {
                    SetNewWaypoint();
                    agent.speed = patrolSpeed;
                }

                break;

            case EnemyState.Dizzy:

                dizzinessCounter += Time.deltaTime;

                if (dizzinessCounter > dizzinessTimer)
                {
                    dizzinessCounter = 0;
                    currentState = EnemyState.Patrol;
                    break;
                }

                break;

            case EnemyState.Dead:

                agent.GetComponent<BasicEnemyAgentAi>().enabled = false;
                agent.GetComponent<CapsuleCollider>().enabled = false;
                weapon.GetComponent<BoxCollider>().enabled = false;
                agent.GetComponent<Animator>().enabled = false;

                gameObject.transform.GetChild(2).GetChild(0).gameObject.SetActive(false);
                gameObject.transform.GetChild(2).GetChild(1).gameObject.SetActive(false);
                arancini.gameObject.SetActive(false);
                PowerUpManager.SpawnPowerUpRandom(transform.position);
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
                weapon.GetComponent<BoxCollider>().enabled = false;
                anim.SetBool("Attack", false);
                agent.isStopped = true;

                if (stunnManager.CurrentStunn < 1)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Patrol;
                    arancini.gameObject.SetActive(false);
                    stunnManager.IsStunned = false;
                    agent.isStopped = false;
                }

                break;
        }
    }
}

