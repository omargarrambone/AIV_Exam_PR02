using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BasicEnemyAgentAi : MonoBehaviour
{
    public NavMeshAgent agent;
    public List<Transform> patrolWaypoints;
    public Transform playerTarget;
    public float activationDistance = 5f;
    public int currentWaypoint;


    // Start is called before the first frame update
    void Start()
    {
        SetNewWaypoint();
    }

    // Update is called once per frame
    void Update()
    {
        float distanceFromTarget = Vector3.Distance(playerTarget.position, agent.transform.position);
        if (distanceFromTarget <= activationDistance)
        {
            agent.SetDestination(playerTarget.position);
        }
        else if (agent.remainingDistance < 0.5f)
        {
            SetNewWaypoint();
        }
    }

    private void FixedUpdate()
    {
        //if (agent.remainingDistance < 0.5f)
        //{
        //    SetNewWaypoint();
        //}
    }

    public void SetNewWaypoint()
    {
        currentWaypoint = Random.Range(0, patrolWaypoints.Count);
        agent.SetDestination(patrolWaypoints[currentWaypoint].position);
    }
}
