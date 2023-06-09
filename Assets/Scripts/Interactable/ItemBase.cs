using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]

public class ItemBase : MonoBehaviour
{
    [SerializeField] private ItemType itemType;
    public UnityEvent onPickUp;

    public void PickUp()
    {
        onPickUp.Invoke();
        InventoryManager.AddItem(itemType);
        gameObject.SetActive(false);
    }
}
