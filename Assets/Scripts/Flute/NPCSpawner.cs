using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public static int PurifiedEnemies;
    public static int KilledEnemies;

    public GameObject[] PurificatedVillagers;

    private void Start()
    {
        for (int i = 0; i < PurificatedVillagers.Length; i++)
        {
            PurificatedVillagers[i].SetActive(false);
        }

        SetVillagersActive();
    }

    [ContextMenu("SetVillagersActive")]
    public void SetVillagersActive()
    {
        int purificatedEnemies = PurifiedEnemies;

        if (purificatedEnemies > PurificatedVillagers.Length) purificatedEnemies = PurificatedVillagers.Length;

        for (int i = 0; i < purificatedEnemies; i++)
        {
            PurificatedVillagers[i].SetActive(true);
            //SetPurificatedAnimation(PurificatedVillagers[i]);
        }
    }

    public void SetPurificatedAnimation(GameObject go)
    {
         go.GetComponent<Animator>().SetBool("Purificated", true);
    }
}
