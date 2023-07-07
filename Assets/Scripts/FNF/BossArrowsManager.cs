using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BossArrowsManager : MonoBehaviour
{

    [SerializeField] private GameObject[] spawnedArrows;
    [SerializeField] private GameObject arrowPrefab;
    [SerializeField] private Transform arrowsParent;
    static public UnityEvent OnBossHittedNote;

    private void Awake()
    {
        OnBossHittedNote = new UnityEvent();
    }

    public void Init(int arrowsToSpawn)
    {
        spawnedArrows = new GameObject[arrowsToSpawn];
    }

    public void SpawnArrow(int index, Vector3 position, Quaternion rotation)
    {
        spawnedArrows[index] = Instantiate(arrowPrefab, arrowsParent);
        spawnedArrows[index].transform.localPosition = position;
        spawnedArrows[index].transform.localRotation = rotation;
    }

    public void TraslateArrows(Vector3 vector)
    {
        arrowsParent.Translate(vector);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if(other.CompareTag("RythmArrow"))
        {
            Destroy(other.gameObject);
            OnBossHittedNote.Invoke();
        }
    }

    private void OnDestroy()
    {
        OnBossHittedNote = null;
    }
}
