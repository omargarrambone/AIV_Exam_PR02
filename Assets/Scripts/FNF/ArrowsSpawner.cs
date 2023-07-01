using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedArrows;
    [SerializeField] private int arrowsToSpawn;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowsParent;

    // Start is called before the first frame update
    void Start()
    {
        spawnedArrows = new GameObject[arrowsToSpawn];

        for (int i = 0; i < arrowsToSpawn; i++)
        {
            spawnedArrows[i] = Instantiate(arrowPrefab, arrowsParent);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
