using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]

public class ItemBase : MonoBehaviour
{
    [SerializeField] bool initWeaponsManager;
    [SerializeField] private GameObject chatBubble;
    static private WeaponsManager weaponsManager;
    [SerializeField] private ItemType itemType;
    public UnityEvent onPickUp;

    private void Start()
    {
        if(initWeaponsManager)
        {
            weaponsManager = FindObjectOfType<WeaponsManager>();
            Destroy(gameObject);
            return;
        }

        if (weaponsManager.TakenWeapons[((int)itemType)])
        {
            gameObject.SetActive(false);
        }
    }

    public void PickUp()
    {
        onPickUp.Invoke();
        weaponsManager.AddItem(itemType);
        gameObject.SetActive(false);
    }

    public void Interact()
    {
        if (chatBubble.gameObject.activeSelf == false)
        {
            chatBubble.SetActive(true);
            StartCoroutine(RemoveAfterSeconds(2));
        }
    }

    IEnumerator RemoveAfterSeconds(int seconds)
    {
        yield return new WaitForSeconds(seconds);
        chatBubble.SetActive(false);
    }
}
