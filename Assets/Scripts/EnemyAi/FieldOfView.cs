using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float radius;

    public float angle = 110f;

    public LayerMask targetMask;
    public LayerMask obstacleMask;

    public bool canSeePlayer;

    private float timerCheck = 0.5f;

    // Start is called before the first frame update
    private void Start()
    {
        
    }

    private void Update()
    {
        timerCheck -= Time.deltaTime;
        if (timerCheck <= 0)
        {
            targetCheck();
            timerCheck = 0.5f;
        }
    }


    public bool targetCheck()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (hitColliders.Length != 0)
        {
            Transform target = hitColliders[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle * 0.5f)
            {
                //Debug.Log(Time.deltaTime);
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask))
                {
                    canSeePlayer = true;
                    //Debug.Log("I see You!");
                    return canSeePlayer;
                }
                else
                {
                    canSeePlayer = false;
                }
            }
            else
            {
                canSeePlayer = false;
            }
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
        return canSeePlayer;

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
        

    //}



}
