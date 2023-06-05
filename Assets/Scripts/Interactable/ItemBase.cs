using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Interactable))]

public class ItemBase : MonoBehaviour
{
    public UnityEvent onPickUp;

    public void PickUp()
    {
        onPickUp.Invoke();
        gameObject.SetActive(false);
    }
}
