using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ArancinoScript : MonoBehaviour
{
    [SerializeField] bool isGoingToTarget;
    public Transform throwThingPosition,ownerPositon, targetPosition;
    [SerializeField] Vector3 playerTargetOffset;
    [SerializeField] float distanceToHitTarget,distanceToHitOwner,throwThingSpeedMovement, throwThingSpeedRotation;
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

        if (isGoingToTarget)
        {
            float distance = Vector3.Distance(transform.position, targetPosition.position);
            Debug.Log(distance);
            if (distance< distanceToHitTarget)
            {
                OnHitTarget.Invoke();
                gameObject.SetActive(false);
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, ownerPositon.position) < distanceToHitOwner)
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
