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
    public WeaponsManager weaponsManager;
    [Range(0f,2f)]
    public float slashPlayRate;


    public void CallOnStartHit()
    {
            weaponsManager.SetObjectsColliders(true);
            OnStartAttack.Invoke();

            swordSlash[weaponsManager.CurrentSlotIndex].playRate = slashPlayRate;
            swordSlash[weaponsManager.CurrentSlotIndex].Play();
    }

    public void CallOnEndHit()
    {
        weaponsManager.SetObjectsColliders(false);

        swordSlash[weaponsManager.CurrentSlotIndex].Stop();
        OnEndAttack.Invoke();
    }
}
