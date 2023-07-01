using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowsSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] spawnedArrows;
    [SerializeField] private int arrowsToSpawn;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowsParent;
    [SerializeField] private float arrowsMovementSpeed;

    private void Awake()
    {
        //remaningArrows = arrowsToSpawn;

    }

    void Start()
    {
        spawnedArrows = new GameObject[arrowsToSpawn];

        for (int i = 0; i < arrowsToSpawn; i++)
        {
            spawnedArrows[i] = Instantiate(arrowPrefab, arrowsParent);

            spawnedArrows[i].transform.Translate(Vector3.down * (i * 3f));

            int myDirection = Random.Range(0, 4);

            // 0 = 

            spawnedArrows[i].transform.Translate(Vector3.right * myDirection * 1.5f);
            spawnedArrows[i].transform.Rotate(new Vector3(0, 0, myDirection * 90));
        }
    }

    void Update()
    {
        arrowsParent.Translate(Vector3.up * Time.deltaTime * arrowsMovementSpeed);
    }
}
