using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static GameObject playerTransform { get; private set; }
    void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player");
    }

}
