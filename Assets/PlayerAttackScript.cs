using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.VFX;

public class PlayerAttackScript : MonoBehaviour
{
    public UnityEvent OnStartAttack, OnEndAttack;
    public VisualEffect[] swordSlash;


    public void CallOnStartHit()
    {
           
            InventoryManager.SetObjectsColliders(true);
            OnStartAttack.Invoke();

            swordSlash[InventoryManager.CurrentSlotIndex].playRate = 0.80f;
            swordSlash[InventoryManager.CurrentSlotIndex].Play();

        
    }

    public void CallOnEndHit()
    {
        InventoryManager.SetObjectsColliders(false);

        swordSlash[InventoryManager.CurrentSlotIndex].Stop();
        OnEndAttack.Invoke();

    }
}
