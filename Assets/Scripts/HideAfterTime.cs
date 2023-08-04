using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAfterTime : MonoBehaviour
{
    [SerializeField] float timer, counter;

    void OnEnable()
    {
        counter = timer;
    }


    void Update()
    {
        counter -= Time.deltaTime;

        if (counter < 0)
        {
            gameObject.SetActive(false);
        }
    }
}
