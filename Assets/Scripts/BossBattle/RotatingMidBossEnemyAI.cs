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
        dizzinessTimer = 4f; // random float 4 - 7

        stunnManager.StunnDecreaseVelocity = 30.0f;

    }

    protected override void Update()
    {
        anim.SetFloat("Speed", agent.velocity.magnitude);

        switch (currentState)
        {
            case EnemyState.Patrol:

                dizzinessCounter += Time.deltaTime;

                if (dizzinessCounter > dizzinessTimer)
                {
                    stunnManager.CurrentStunn = 100;
                    dizzinessCounter = 0;
                    currentState = EnemyState.Stun;
                    break;
                }

                if (agent.remainingDistance < 2f)
                {
                    SetNewWaypoint();
                    agent.speed = patrolSpeed;
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
                agent.speed = 0;

                if (stunnManager.CurrentStunn < 1)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Patrol;
                    arancini.gameObject.SetActive(false);
                    stunnManager.IsStunned = false;
                    SetNewWaypoint();
                }

                break;
        }
    }
}

