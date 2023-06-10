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
    


    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        anim = GetComponent<Animator>();       
        currentState = EnemyState.Patrol;
        SetNewWaypoint();
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
                if (agent.remainingDistance < 2f)
                {
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    SetNewWaypoint();
                    break;
                }
                else if (fov.targetCheck() == true)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                break;
            case EnemyState.Chase:
                if (fov.targetCheck() == false)
                {
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
                    break;
                }
                Debug.Log("Attack");
                break;
            case EnemyState.Healing:
                break;
            case EnemyState.Stun:
                // Enemy ready to be purified by the sound of the Magic Flute
                break;
            default:
                break;
        }

        if (agent.remainingDistance < 2f)
        {
            currentState = EnemyState.Patrol;
            agent.speed = patrolSpeed;
            SetNewWaypoint();            
        }
    }


    public void SetNewWaypoint()
    {
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);
        //distanceFromWaypoint = Vector3.Distance(agent.transform.position, patrolWaypoints[currentWaypoint].position);
        agent.SetDestination(patrolWaypoints[currentWaypoint].position);
    }
}
