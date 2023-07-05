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
    [SerializeField] CameraLockWall attackCamera;
    [SerializeField] bool isAttacking;
    [SerializeField] int currentAttack, maxAttack;
    [SerializeField] float attackTimer, attackCounter, rotationSpeed;
    [SerializeField] private float dizzinessCounter, dizzinessTimer, minDizzinessTimer, maxDizzinessTimer;
    [Header("PhaseMinions Variables")]
    [SerializeField] CameraLockWall minionCamera;
    [SerializeField] bool isThrowing;
    [SerializeField] ParticleSystem dissolveEffect;
    [SerializeField] Transform teleportTransform, throwThingTransform;
    [SerializeField] GameObject[] enemyGhostPrefabs;
    [SerializeField] Transform[] spawnPosition;
    [SerializeField] ArancinoScript throwThingRef;
    [SerializeField] float healthRechargeSpeed, healthRechargeSpeedIncreaseSpeed, dissolveTimer, dissolveCounter,damageArancinoToMyself;
    [SerializeField] int maxMinions, currentMinions, leftMinions;
    [Header("PhaseRythm Variables")]
    [SerializeField] Camera rythmCamera;
    [SerializeField] SongManager songManager;
    [SerializeField] Transform rythmTransform;

    protected override void Start()
    {
        base.Start();
        SetWeaponsCollider(true);

        fov.Angle = 360;

        stunnManager.StunnDecreaseVelocity = 50.0f;
        stunnManager.Timer = 2f;
        stunnManager.IsImmune = true;

        SetImmortal();

        parryTimer = 0f;

        currentState = EnemyState.Chase;
        agent.speed = chaseSpeed;


        throwThingRef.OnHitOwner.AddListener(ResetMinionsSpawn);
        throwThingRef.OnHitOwner.AddListener(() => { healthManager.TakeDamage(damageArancinoToMyself); });
        throwThingRef.OnHitOwner.AddListener(SetImmortal);

        throwThingRef.OnHitTarget.AddListener(ResetMinionsSpawn);
        throwThingRef.OnHitTarget.AddListener(SetImmortal);


        throwThingRef.ownerPositon = transform;

        attackCamera.enabled = true;
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

                    if(currentAttack < 0)
                    {
                        isAttacking = false;
                        anim.SetBool("IsAttacking", false);

                        if (!isAttacking)
                        {
                            SetWeaponsCollider(false);
                            PauseMovement();
                            currentState = EnemyState.Dizzy;
                            SetMortal();
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
                    ResumeMovement();
                    weapon.gameObject.SetActive(true);
                    SetWeaponsCollider(true);

                    stunnManager.IsImmune = true;
                    stunnManager.SetStun(0);
                    SetImmortal();
                    break;
                }

                break;

            case EnemyState.Dead:
                anim.SetBool("IsAttacking", false);
                currentPhase = FinalBossPhase.PhaseMinions;
                currentState = EnemyState.Healing;
                SetImmortal();
                anim.SetBool("Stunned", true);
                anim.SetBool("IsDizzy", false);
                isAttacking = false;
                ParticleSystem dis = Instantiate(dissolveEffect, transform.position, dissolveEffect.transform.rotation);
                dis.transform.localScale = transform.localScale;
                Destroy(dis.gameObject, 1);
                PowerUpManager.SpawnPowerUpRandom(transform.position);
                PauseMovement();
                gameObject.transform.SetPositionAndRotation(teleportTransform.position, teleportTransform.rotation);
                leftMinions = maxMinions;
                attackCamera.CallOnExit();
                minionCamera.CallOnEnter();
                break;
        }
    }

    private void ResetMinionsSpawn()
    {
        if (healthManager.IsImmune) return;

        currentMinions = 0;
        currentState = EnemyState.Attack;
        
        anim.SetBool("IsSpawning", true);
        anim.SetBool("IsThrowing", false);

        isThrowing = false;
    }

    private void PauseMovement()
    {
        agent.isStopped = true;
        agent.enabled = false;
    }

    private void ResumeMovement()
    {
        agent.isStopped = false;
        agent.enabled = true;
    }

    private void SetImmortal()
    {
        healthManager.IsImmune = true;
        skinnedMeshRenderer.material = immortalMaterial;
    }

    private void SetMortal()
    {
        healthManager.IsImmune = false;
        skinnedMeshRenderer.material = baseMaterial;
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

                if (leftMinions <= 0 && !isThrowing)
                {
                    isThrowing = true;
                    anim.SetBool("IsThrowing", true);
                    leftMinions = maxMinions;
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

                    dissolveCounter += Time.deltaTime;

                    if (dissolveCounter > dissolveTimer)
                    {
                        currentState = EnemyState.Attack;
                        anim.SetBool("IsSpawning", true);
                    }
                }

                break;
            case EnemyState.Dead:
                anim.SetBool("IsSpawning", false);
                currentPhase = FinalBossPhase.PhaseRythm;
                anim.SetTrigger("Dance");
                songManager.PlaySong();
                SetMortal();
                currentState = EnemyState.Attack;
                GoToRythmTransform();
                rythmCamera.transform.position = Camera.main.transform.position;
                rythmCamera.transform.rotation = Camera.main.transform.rotation;
                Camera.main.gameObject.SetActive(false);
                rythmCamera.gameObject.SetActive(true);
                PlayerManager.HidePlayerCanvas();
                break;
            case EnemyState.Dizzy:
                break;
        }

    }

    public void GoToRythmTransform()
    {
        transform.position = rythmTransform.position;
        transform.rotation = rythmTransform.rotation;
    }

    private void PhaseRythm()
    {
        //switch (currentState)
        //{
        //    case EnemyState.Patrol:
        //        break;
        //    case EnemyState.Chase:
        //        break;
        //    case EnemyState.Attack:
        //        break;
        //    case EnemyState.Stun:
        //        break;
        //    case EnemyState.Healing:
        //        break;
        //    case EnemyState.Dead:
        //        break;
        //    case EnemyState.Dizzy:
        //        break;
        //}
    }

    public void SpawnEnemy()
    {
        if (currentMinions < maxMinions)
        {
                GameObject enemy = Instantiate(enemyGhostPrefabs[Random.Range(0, enemyGhostPrefabs.Length)], spawnPosition[Random.Range(0, spawnPosition.Length)].position, Quaternion.Euler(0,180,0));
                BasicEnemyAgentAi ai = enemy.GetComponent<BasicEnemyAgentAi>();
                ai.SetWaypoints(patrolWaypoints);
                HealthManager hm = enemy.GetComponent<HealthManager>();
                hm.OnDeath.AddListener(() => { leftMinions--; });

                currentMinions++;

                if (currentMinions >= maxMinions)
                {
                    anim.SetBool("IsSpawning", false);
                }
        }
    }

    public void ThrowKunai()
    {
        throwThingRef.gameObject.SetActive(true);
        throwThingRef.gameObject.transform.position = throwThingTransform.position;
        throwThingRef.targetPosition = playerTarget;
        Vector3 distanceFromTarget = (playerTarget.position) - throwThingRef.transform.position;
        throwThingRef.transform.forward = distanceFromTarget;
        SetMortal();
    }
}

