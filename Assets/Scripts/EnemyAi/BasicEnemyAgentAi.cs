using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] protected EnemyState currentState;
    [SerializeField] protected float chaseSpeed;
    [SerializeField] protected float patrolSpeed;
    [SerializeField] [Range(0f,1f)] protected float parryChance = 0.3f;
    [SerializeField] protected float attackDistance;

    [Header("References")]
    [SerializeField] protected NavMeshAgent agent;
    [SerializeField] protected GameObject weapon;
    [SerializeField] protected StunnManager stunnManager;
    [SerializeField] protected HealthManager healthManager;
    [SerializeField] protected Animator anim;
    [SerializeField] protected ParticleSystem arancini;
    [SerializeField] protected EnemyDamageManager enemyDamageManager;
    [SerializeField] protected FieldOfView fov;
    [SerializeField] protected List<Transform> patrolWaypoints;
    [SerializeField] protected GameObject healthBar;
    [SerializeField] protected GameObject stunBar;
    protected CapsuleCollider enemyCollider;
    protected Transform playerTarget;
    protected int currentWaypoint;


    // Start is called before the first frame update
    virtual protected void Start()
    {
        fov = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();
        enemyCollider = GetComponent<CapsuleCollider>();
        currentState = EnemyState.Patrol;
        weapon.GetComponent<BoxCollider>().enabled = false;
        playerTarget = PlayerManager.PlayerGameObject.transform;
    }

    // Update is called once per frame
    virtual protected void Update()
    {
        Vector3 distanceFromTarget = playerTarget.position - agent.transform.position;
        
        anim.SetFloat("Speed", agent.velocity.magnitude);

        switch (currentState)
        {
            case EnemyState.Patrol:

                if (fov.targetCheck() == true)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                else if (agent.remainingDistance < 2f)
                {
                    fov.Angle = 150;
                    SetNewWaypoint();
                    agent.speed = patrolSpeed;                 
                    anim.SetBool("Attack", false);
                    break;
                }
                if (enemyDamageManager.PlayerIsAttacking || enemyDamageManager.IsHitting)              
                {
                    currentState = EnemyState.Chase;
                    
                    break;
                }
               
                break;


            case EnemyState.Chase:               

                fov.Angle = 360;             

                if (fov.targetCheck() == true && distanceFromTarget.magnitude <= attackDistance)
                {
                    agent.speed = 0;                    
                    currentState = EnemyState.Attack;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    //Fov.Angle = 150;
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    
                    break;
                }

                agent.speed = chaseSpeed;
                agent.SetDestination(playerTarget.position);
                break;



            case EnemyState.Attack:

                anim.SetBool("Attack", true);
              
                agent.transform.forward = new Vector3(distanceFromTarget.normalized.x, 0,distanceFromTarget.normalized.z);

                if (enemyDamageManager.IsParrying)
                {
                    anim.SetBool("IsParrying", true);
                    parryChance -= Time.deltaTime;
                 
                    if (parryChance <= 0)
                    {
                        enemyDamageManager.IsParrying = false;
                        anim.SetBool("IsParrying", false);
                        parryChance = 0.3f;
                    }
                    break;
                }

                if (enemyDamageManager.IsHitting && stunnManager.IsStunned == false)
                {
                    enemyDamageManager.IsHitting = false;
                    break;
                }

                if (fov.targetCheck() == true && distanceFromTarget.magnitude > attackDistance)
                {
                    anim.SetBool("Attack", false);
                    agent.speed = chaseSpeed;
                    currentState = EnemyState.Chase;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    break;
                }

                break;


            case EnemyState.Healing:
                break;

            case EnemyState.Dead:
               
                this.enabled = false;
                weapon.SetActive(false);
                anim.enabled = false;
                enemyCollider.enabled = false;                
                healthBar.SetActive(false);
                stunBar.SetActive(false);
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

    public void SetState(int state)
    {
        currentState = (EnemyState)state;
    }


    public void SetNewWaypoint()
    {
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);
        agent.SetDestination(patrolWaypoints[currentWaypoint].position);
    }

    public void StartAttack()
    {
        weapon.GetComponent<BoxCollider>().enabled = true;
    }

    public void EndAttack()
    {
        weapon.GetComponent<BoxCollider>().enabled = false;
    }

}
