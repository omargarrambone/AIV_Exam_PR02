using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]

public class ItemBase : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    public UnityEvent onPickUp;

    private void Start()
    {
        foreach (var item in InventoryManager.InventoryItems)
        {
            if(item.ItemType == itemType && item.IsTaken == true)
            {
                gameObject.SetActive(false);
            }
        }
    }

    public void PickUp()
    {
        onPickUp.Invoke();
        InventoryManager.AddItem(itemType);
        gameObject.SetActive(false);
    }
}
