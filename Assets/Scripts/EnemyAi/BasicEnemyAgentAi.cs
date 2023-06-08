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



    // Start is called before the first frame update
    void Start()
    {
        fov = GetComponent<FieldOfView>();
        SetNewWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        //float distanceFromTarget = Vector3.Distance(playerTarget.position, agent.transform.position);
        //if (fov.targetCheck() == true && distanceFromTarget <= activationDistance)
        
        if (fov.targetCheck() == true)
        {
            agent.speed = chaseSpeed;
            agent.SetDestination(playerTarget.position);
        }
        else if (agent.remainingDistance < 0.5f)
        {
            agent.speed = patrolSpeed;
            SetNewWaypoint();
        }
    }


    public void SetNewWaypoint()
    {
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);
        agent.SetDestination(patrolWaypoints[currentWaypoint].position);
    }
}
