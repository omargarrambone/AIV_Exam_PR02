using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArancinoScript : MonoBehaviour
{
    [SerializeField] bool isGoingToTarget;
    public Transform throwThingPosition,ownerPositon, targetPosition;
    [SerializeField] Vector3 playerTargetOffset;
    [SerializeField] float distanceToHit,throwThingSpeedMovement, throwThingSpeedRotation;
    public UnityEvent OnHitOwner, OnHitTarget;

    private void OnEnable()
    {
        isGoingToTarget = true;
    }

    private void Update()
    {
        Vector3 distanceFromTarget = (targetPosition.position + playerTargetOffset) - transform.position;
        transform.forward = distanceFromTarget;

        transform.position += transform.forward * throwThingSpeedMovement * Time.deltaTime;

        if(Vector3.Distance(transform.position, targetPosition.position) < distanceToHit)
        {
            if (isGoingToTarget)
            {
                OnHitTarget.Invoke();
            }
            else
            {
                OnHitOwner.Invoke();
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword" || other.gameObject.tag == "Kick")
        {
            isGoingToTarget = false;
            targetPosition = ownerPositon;
        }
    }
}
