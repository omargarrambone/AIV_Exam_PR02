using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public float patrolSpeed;
    public float chaseSpeed;
    public List<Transform> patrolWaypoints;
    public Transform playerTarget;
    public float attackDistance;
    public int currentWaypoint;
    public FieldOfView fov;
    public EnemyState currentState;
    public Animator anim;
    public bool isStunned;
    public float stunTimer;




    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();       
        currentState = EnemyState.Patrol;
        //SetNewWaypoint();

        playerTarget = PlayerManager.playerTransform.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromTarget = Vector3.Distance(playerTarget.position, agent.transform.position);
        //if (fov.targetCheck() == true && distanceFromTarget <= activationDistance)        
        anim.SetFloat("Speed", agent.velocity.magnitude);

        switch (currentState)
        {
            case EnemyState.Patrol:
                if (fov.targetCheck() == true)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                else if(agent.remainingDistance < 2f)
                {
                    //currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    SetNewWaypoint();
                    break;
                }                 
                break;


            case EnemyState.Chase:
                //fov.angle = 360;
                if (fov.targetCheck() == false)
                {
                    //fov.angle = 150;
                    currentState = EnemyState.Patrol;
                    break;
                }
                else if (fov.targetCheck() == true && distanceFromTarget <= attackDistance)
                {
                    agent.speed = 0;
                    currentState = EnemyState.Attack;
                    break;
                }
                agent.speed = chaseSpeed;
                agent.SetDestination(playerTarget.position);
                break;


            case EnemyState.Attack:
                if (isStunned)
                {
                    currentState = EnemyState.Stun;
                }
                if (fov.targetCheck() == true && distanceFromTarget > attackDistance)
                {
                    currentState = EnemyState.Chase;
                    agent.speed = chaseSpeed;
                    break;
                }
                else if (fov.targetCheck() == false)
                {
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    //break;
                }              
                Debug.Log("Attack");
                break;


            case EnemyState.Healing:
                break;


            case EnemyState.Stun:
                anim.SetBool("Stunned", true);
                //Enemy ready to be purified by the sound of the Magic Flute
                stunTimer -= Time.deltaTime;
                if (stunTimer <= 0)
                {
                    anim.SetBool("Stunned", false);
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    isStunned = false;
                    stunTimer = 7f;                }
                break;
            default:
                break;
        }

        //if (agent.remainingDistance < 2f)
        //{
        //    currentState = EnemyState.Patrol;
        //    agent.speed = patrolSpeed;
        //    SetNewWaypoint();            
        //}
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
}
