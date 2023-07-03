using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalBossEnemyAI : BasicEnemyAgentAi
{
    [SerializeField] FinalBossPhase currentPhase;
    [SerializeField] private Collider[] weapons;


    [SerializeField] private Material baseMaterial, immortalMaterial;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;

    [Header("PhaseAttack Variables")]
    [SerializeField] bool isAttacking;
    [SerializeField] int currentAttack, maxAttack;
    [SerializeField] float attackTimer, attackCounter, rotationSpeed;
    [SerializeField] private float dizzinessCounter, dizzinessTimer, minDizzinessTimer, maxDizzinessTimer;
    [Header("PhaseMinions Variables")]
    [SerializeField] GameObject[] enemyGhostPrefabs;
    [SerializeField] float healthRechargeSpeed,spawnTimer, spawnCounter;
    [SerializeField] int currentMinions, maxMinions;
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

        skinnedMeshRenderer.material = immortalMaterial;

        parryChance = 0f;

        currentState = EnemyState.Chase;
        agent.speed = chaseSpeed;
    }

    private void SetRandomDizziness()
    {
        dizzinessCounter = Random.Range(minDizzinessTimer, maxDizzinessTimer);
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
        anim.SetFloat("Speed", agent.velocity.magnitude);

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
                    isAttacking = true;
                    currentState = EnemyState.Attack;
                    attackCounter = 0;
                    currentAttack = maxAttack;
                    break;
                }

                agent.SetDestination(playerTarget.position);
                break;

            case EnemyState.Attack:

                attackCounter -= Time.deltaTime;
                agent.SetDestination(playerTarget.position);
                agent.transform.forward = new Vector3(distanceFromTarget.x, 0, distanceFromTarget.z);

                if (attackCounter < 0)
                {
                    attackCounter = attackTimer;
                    anim.SetBool("IsAttacking",true);

                    currentAttack--;

                    if(currentAttack < 1)
                    {
                        isAttacking = false;
                        anim.SetBool("IsAttacking", false);

                        if (!isAttacking)
                        {
                            SetWeaponsCollider(false);
                            agent.isStopped = true;
                            currentState = EnemyState.Dizzy;
                            healthManager.IsImmune = false;
                            skinnedMeshRenderer.material = baseMaterial;
                            anim.SetBool("IsDizzy",true);
                            SetRandomDizziness();
                            break;
                        }
                    }
                }

                break;

            case EnemyState.Dizzy:

                dizzinessCounter -= Time.deltaTime;

                if (dizzinessCounter < 0)
                {
                    SetRandomDizziness();
                    anim.SetBool("IsDizzy", false);
                    currentState = EnemyState.Chase;
                    agent.isStopped = false;
                    weapon.gameObject.SetActive(true);
                    healthManager.IsImmune = true;
                    SetWeaponsCollider(true);

                    stunnManager.IsImmune = true;
                    stunnManager.SetStun(0);
                    skinnedMeshRenderer.material = immortalMaterial;
                    break;
                }

                break;

            case EnemyState.Dead:
                currentPhase = FinalBossPhase.PhaseMinions;
                PowerUpManager.SpawnPowerUpRandom(transform.position);
                currentState = EnemyState.Healing;
                healthManager.IsImmune = true;
                skinnedMeshRenderer.material = immortalMaterial;
                anim.SetBool("Stunned", true);
                break;
        }
    }

    private void PhaseMinions()
    {
        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;

        switch (currentState)
        {
            case EnemyState.Patrol:
                break;
            case EnemyState.Chase:
                break;
            case EnemyState.Attack:

                if (currentMinions < maxMinions)
                {
                    spawnCounter -= Time.deltaTime;

                    if(spawnCounter < 0)
                    {
                        spawnCounter = spawnTimer;

                        GameObject enemy = Instantiate(enemyGhostPrefabs[Random.Range(0, enemyGhostPrefabs.Length)]);

                        enemy.GetComponent<BasicEnemyAgentAi>().SetWaypoints(patrolWaypoints);

                        currentMinions++;
                    }
                }
                break;
            case EnemyState.Stun:
                break;
            case EnemyState.Healing:

                healthManager.AddHealth(Time.deltaTime * healthRechargeSpeed);

                if(healthManager.CurrentHealth >= healthManager.MaxHealth)
                {
                    healthManager.CurrentHealth = healthManager.MaxHealth;
                    currentState = EnemyState.Attack;
                    anim.SetBool("Stunned", false);
                    anim.SetBool("IsSpawning", true);
                }

                break;
            case EnemyState.Dead:
                anim.SetBool("IsSpawning", false);
                break;
            case EnemyState.Dizzy:
                break;
            case EnemyState.LAST:
                break;
            default:
                break;
        }

    }

    private void PhaseRythm()
    {

    }
}

