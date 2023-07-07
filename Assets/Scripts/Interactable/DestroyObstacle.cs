using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyObstacle : MonoBehaviour
{
    public GameObject Obstacle;
    public GameObject BrokenObstacle;

    public AudioSource WallBrake;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Rod")
        {
            WallBrake.Play();
            Obstacle.SetActive(false);
            BrokenObstacle.SetActive(true);
            Destroy(BrokenObstacle, 3f);
            this.GetComponent<Collider>().enabled = false;

        }
    }
}
