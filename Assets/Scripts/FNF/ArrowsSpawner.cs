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
    [SerializeField] private float noteSpawnOffset;
    [SerializeField] private BossArrowsManager bossArrowsManager;

    void Start()
    {
        spawnedArrows = new GameObject[arrowsToSpawn];

        bossArrowsManager.Init(arrowsToSpawn);

        for (int i = 0; i < arrowsToSpawn; i++)
        {
            spawnedArrows[i] = Instantiate(arrowPrefab, arrowsParent);

            spawnedArrows[i].transform.Translate(Vector3.down * (i * noteSpawnOffset));

            int myDirection = Random.Range(0, 4);

            int newRotation = 0;

            switch (myDirection)
            {
                case 0:
                    newRotation = -90;
                    break;
                case 1:
                    newRotation = 0;
                    break;
                case 2:
                    newRotation = 180;
                    break;
                case 3:
                    newRotation = 90;
                    break;
            }

            spawnedArrows[i].transform.Translate(Vector3.right * myDirection * 1.5f);
            spawnedArrows[i].transform.Rotate(new Vector3(0, 0, newRotation));

            bossArrowsManager.SpawnArrow(i,spawnedArrows[i].transform.localPosition, spawnedArrows[i].transform.localRotation);
        }
    }

    void Update()
    {
        arrowsParent.Translate(Vector3.up * Time.deltaTime * arrowsMovementSpeed);
        bossArrowsManager.TraslateArrows(Vector3.up * Time.deltaTime * arrowsMovementSpeed);
    }
}
