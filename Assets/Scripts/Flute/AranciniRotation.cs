using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AranciniRotation : MonoBehaviour
{
    static Vector3 rotationVector = new Vector3(0f,0f,90f);
    void Update()
    {
        transform.Rotate(rotationVector * Time.deltaTime);
    }
}
