using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject BrokenObstacle;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sword")
        {
            Obstacle.SetActive(false);
            BrokenObstacle.SetActive(true);
            Destroy(BrokenObstacle, 3f);
        }
    }
}
