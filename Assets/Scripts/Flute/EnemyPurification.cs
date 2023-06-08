using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPurification : MonoBehaviour
{
    public bool IsStunned;
    public Transform PurificatedLocation;
    private Animator Animator;
    private static int purificatedEnemies;

    private GameObject[] purificatedVillagers;

    private void Start()
    {
        purificatedEnemies = PlayerPrefs.GetInt("PurificatedEnemies");

        if(purificatedEnemies < 5)
        {
            purificatedVillagers[0].SetActive(true);
        }

        if (purificatedEnemies < 10)
        {

        }
    }

    public void SetPurificatedAnimation()
    {
        Animator.SetBool("purificated", true);
    }
}
