using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KunaiLogic : MonoBehaviour
{
    [SerializeField] private LayerMask mask;
    [SerializeField] private float hitDistance;  
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, 4f);
    }


    private void FixedUpdate()
    {
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, hitDistance, mask))
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.green);
            Destroy(this.gameObject);
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hitDistance, Color.red);
            //Debug.Log("Did not Hit");
        }
    }

}
