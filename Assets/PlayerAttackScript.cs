using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayerAttackScript : MonoBehaviour
{
    public UnityEvent OnStartAttack, OnEndAttack;
    public VisualEffect swordSlash;


    public void CallOnStartHit()
    {


        if (InventoryManager.CurrentSlotIndex == 0)
        {
            InventoryManager.SetObjectsColliders(true);
            OnStartAttack.Invoke();

        }
        else
        {
            InventoryManager.SetObjectsColliders(true);
            OnStartAttack.Invoke();

            swordSlash.playRate = 0.80f;
            swordSlash.Play();

        }
    }

    public void CallOnEndHit()
    {
        InventoryManager.SetObjectsColliders(false);

        swordSlash.Stop();
        OnEndAttack.Invoke();

    }
}
