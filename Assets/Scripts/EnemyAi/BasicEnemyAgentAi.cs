using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public float patrolSpeed = 3;
    public float chaseSpeed = 6;
    public List<Transform> patrolWaypoints;
    public Transform playerTarget;
    //public float activationDistance = 5f;
    public int currentWaypoint;
    public FieldOfView fov;
    public EnemyState currentState;



    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        currentState = EnemyState.Patrol;
        SetNewWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        //float distanceFromTarget = Vector3.Distance(playerTarget.position, agent.transform.position);
        //if (fov.targetCheck() == true && distanceFromTarget <= activationDistance)        
       
        switch (currentState)
        {
            case EnemyState.Patrol:
                if (fov.targetCheck() == true)
                {
                    currentState = EnemyState.Chase;
                    break;
                }
                else if (agent.remainingDistance < 0.5f)
                {
                    currentState = EnemyState.Patrol;
                    agent.speed = patrolSpeed;
                    SetNewWaypoint();
                }               
                break;
            case EnemyState.Chase:
                if (fov.targetCheck() == false)
                {
                    currentState = EnemyState.Patrol;
                    break;
                }
                agent.speed = chaseSpeed;
                agent.SetDestination(playerTarget.position);
                break;
            case EnemyState.Attack:
                break;
            case EnemyState.Healing:
                break;
            case EnemyState.Stun:
                // Enemy ready to be purified by the sound of the Magic Flute
                break;
            default:
                break;
        }
    }


    public void SetNewWaypoint()
    {
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);
        agent.SetDestination(patrolWaypoints[currentWaypoint].position);
    }
}
