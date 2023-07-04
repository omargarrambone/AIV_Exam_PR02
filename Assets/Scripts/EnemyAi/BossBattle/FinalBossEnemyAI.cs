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
    [SerializeField] bool isThrowing;
    [SerializeField] ParticleSystem dissolveEffect;
    [SerializeField] Transform teleportTransform, throwThingTransform;
    [SerializeField] GameObject[] enemyGhostPrefabs;
    [SerializeField] GameObject throwThingPrefab;
    GameObject throwThingRef;
    [SerializeField] LayerMask playerLayerMask;
    [SerializeField] float throwThingSpeedMovement,healthRechargeSpeed, healthRechargeSpeedIncreaseSpeed,spawnTimer, spawnCounter, dissolveTimer, dissolveCounter;
    [SerializeField] int maxMinions,currentMinions;
    [Header("PhaseRythm Variables")]
    [SerializeField] SongManager songManager;

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

        parryTimer = 0f;

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
                currentState = EnemyState.Healing;
                healthManager.IsImmune = true;
                skinnedMeshRenderer.material = immortalMaterial;
                anim.SetBool("Stunned", true);
                break;
        }
    }

    private void PhaseMinions()
    {
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

                        if(currentMinions >= maxMinions)
                        {
                            anim.SetBool("IsSpawning", false);
                            anim.SetBool("IsThrowing", true);
                        }
                    }
                }

                else if (isThrowing) {
                    Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;
                    throwThingRef.transform.position += distanceFromTarget * throwThingSpeedMovement * Time.deltaTime;

                    if (Physics.CheckSphere(throwThingRef.transform.position, 0.4f, playerLayerMask))
                    {
                        isThrowing = false;
                        anim.SetBool("IsThrowing", false);
                        anim.SetBool("IsSpawning", true);
                        currentMinions = 0;
                    }
                }

                break;
            case EnemyState.Stun:
                break;
            case EnemyState.Healing:

                healthRechargeSpeed += Time.deltaTime * healthRechargeSpeedIncreaseSpeed;

                healthManager.AddHealth(Time.deltaTime * healthRechargeSpeed);

                if(healthManager.CurrentHealth >= healthManager.MaxHealth)
                {
                    healthManager.CurrentHealth = healthManager.MaxHealth;
                    anim.SetBool("Stunned", false);

                    ParticleSystem dis = Instantiate(dissolveEffect, transform.position, dissolveEffect.transform.rotation);
                    dis.transform.localScale = transform.localScale;

                    dissolveCounter += Time.deltaTime;

                    Destroy(dis.gameObject, 1);

                    if(dissolveCounter > dissolveTimer)
                    {
                        PowerUpManager.SpawnPowerUpRandom(transform.position);

                        agent.isStopped = true;
                        agent.enabled = false;
                        gameObject.transform.SetPositionAndRotation(teleportTransform.position, teleportTransform.rotation);
                        currentState = EnemyState.Attack;
                        anim.SetBool("IsSpawning", true);
                    }
                }

                break;
            case EnemyState.Dead:
                anim.SetBool("IsSpawning", false);
                break;
            case EnemyState.Dizzy:
                break;
        }

    }

    private void PhaseRythm()
    {

    }

    public void ThrowKunai()
    {
        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;
        throwThingRef = Instantiate(throwThingPrefab, throwThingTransform.position, Quaternion.Euler(distanceFromTarget));
        isThrowing = true;
    }
}

