using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingMidBossEnemyAI : BasicEnemyAgentAi
{
    [SerializeField] private float dizzinessCounter, dizzinessTimer, minDizzinessTimer,maxDizzinessTimer;
    [SerializeField] private Collider[] weapons;

    protected override void Start()
    {
        base.Start();
        SetWeaponsCollider(true);

        stunnManager.StunnDecreaseVelocity = 50.0f;
        stunnManager.Timer = 2f;
        stunnManager.IsImmune = true;
        healthManager.IsImmune = true;
        SetRandomDizziness();

        parryTimer = 0;
    }

    void SetWeaponsCollider(bool value)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].enabled = value;
        }
    }

    private void SetRandomDizziness()
    {
        dizzinessCounter = Random.Range(minDizzinessTimer, maxDizzinessTimer);
    }

    protected override void Update()
    {
        switch (currentState)
        {
            case EnemyState.Patrol:

                dizzinessCounter -= Time.deltaTime;

                if (dizzinessCounter < 0)
                {
                    SetRandomDizziness();
                    anim.SetBool("IsDizzy", true);
                    currentState = EnemyState.Dizzy;
                    agent.isStopped = true;
                    weapon.gameObject.SetActive(false);
                    healthManager.IsImmune = false;
                    SetWeaponsCollider(false);

                    break;
                }

                if (agent.remainingDistance < 2f)
                {
                    SetNewWaypoint();
                    agent.speed = patrolSpeed;
                }

                break;

            case EnemyState.Dizzy:

                if(healthManager.CurrentHealth < 30)
                {
                    stunnManager.IsImmune = false;
                    stunnManager.SetStun(100);
                    stunnManager.IsStunned = true;
                    anim.SetBool("Stunned", true);
                    anim.SetBool("IsDizzy", false);
                    agent.isStopped = true;
                    arancini.gameObject.SetActive(true);
                    weapon.GetComponent<BoxCollider>().enabled = false;
                    break;
                }

                dizzinessCounter -= Time.deltaTime;

                if (dizzinessCounter < 0)
                {
                    SetRandomDizziness();
                    anim.SetBool("IsDizzy", false);
                    currentState = EnemyState.Patrol;
                    agent.isStopped = false;
                    weapon.gameObject.SetActive(true);
                    healthManager.IsImmune = true;
                    SetWeaponsCollider(true);

                    stunnManager.IsImmune = true;
                    stunnManager.SetStun(0);
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
                ragdollManager.EnableRagdoll();
                Destroy(this.gameObject, 5f);
                break;

            case EnemyState.Stun:

                if (healthManager.IsDead)
                {
                    currentState = EnemyState.Dead;
                    break;
                }

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

