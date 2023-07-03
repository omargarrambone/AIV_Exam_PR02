using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEnemyAI : BasicEnemyAgentAi
{
    [SerializeField] FinalBossPhase currentPhase;
    [SerializeField] private Collider[] weapons;

    [Header("PhaseAttack Variables")]
    [SerializeField] int maxAttack;
    [Header("PhaseMinions Variables")]
    [SerializeField] int test1;
    [Header("PhaseRythm Variables")]
    [SerializeField] int test2;

    protected override void Start()
    {
        base.Start();
        SetWeaponsCollider(true);

        fov.Angle = 360;

        stunnManager.StunnDecreaseVelocity = 50.0f;
        stunnManager.Timer = 2f;
        stunnManager.IsImmune = true;
        healthManager.IsImmune = true;
        //SetRandomDizziness();

        parryChance = 0f;

        currentState = EnemyState.Chase;
        agent.speed = chaseSpeed;
    }

    void SetWeaponsCollider(bool value)
    {
        for (int i = 0; i < weapons.Length; i++)
        {
            weapons[i].enabled = value;
        }
    }


    protected override void Update()
    {
        switch (currentPhase)
        {
            case FinalBossPhase.PhaseAttack:
                PhaseAttack();
                break;
            case FinalBossPhase.PhaseMinions:
                PhaseMinions();
                break;
            case FinalBossPhase.PhaseRythm:
                PhaseRythm();
                break;
        }       
    }

    private void PhaseAttack()
    {
        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;

        switch (currentState)
        {
            case EnemyState.Chase:
                agent.transform.forward = new Vector3(distanceFromTarget.x, 0, distanceFromTarget.z);

                if (distanceFromTarget.magnitude < attackDistance)
                {
                    agent.speed = 0;
                    agent.isStopped = true;
                    currentState = EnemyState.Attack;

                    anim.SetBool("Attack", true);
                    break;
                }

                agent.SetDestination(playerTarget.position);
                break;

            case EnemyState.Attack:

                agent.transform.forward = new Vector3(distanceFromTarget.x, 0, distanceFromTarget.z);

                if (distanceFromTarget.magnitude > attackDistance)
                {
                    agent.speed = chaseSpeed;
                    agent.isStopped = false;
                    currentState = EnemyState.Chase;
                    break;
                }

                break;

            case EnemyState.Dizzy:

                if (healthManager.CurrentHealth < 30)
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

                //dizzinessCounter -= Time.deltaTime;

                //if (dizzinessCounter < 0)
                //{
                //    SetRandomDizziness();
                //    anim.SetBool("IsDizzy", false);
                //    currentState = EnemyState.Patrol;
                //    agent.isStopped = false;
                //    weapon.gameObject.SetActive(true);
                //    healthManager.IsImmune = true;
                //    SetWeaponsCollider(true);

                //    stunnManager.IsImmune = true;
                //    stunnManager.SetStun(0);
                //    break;
                //}

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

    private void PhaseMinions()
    {

    }

    private void PhaseRythm()
    {

    }
}

