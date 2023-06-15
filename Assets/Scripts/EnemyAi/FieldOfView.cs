using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FieldOfView : MonoBehaviour
{
    public float Radius;

    public float Angle;

    public LayerMask TargetMask;
    public LayerMask ObstacleMask;

    public bool CanSeePlayer;

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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, Radius, TargetMask);

        if (hitColliders.Length != 0)
        {
            Transform target = hitColliders[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < Angle * 0.5f)
            {
                //Debug.Log(Time.deltaTime);
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, ObstacleMask))
                {
                    CanSeePlayer = true;
                    //Debug.Log("I see You!");
                    return CanSeePlayer;
                }
                else
                {
                    CanSeePlayer = false;
                }
            }
            else
            {
                CanSeePlayer = false;
            }
        }
        else if (CanSeePlayer)
        {
            CanSeePlayer = false;
        }
        return CanSeePlayer;

    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawWireSphere(transform.position, radius);
        

    //}



}
