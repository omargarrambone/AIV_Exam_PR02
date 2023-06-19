using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackScript : MonoBehaviour
{
    public UnityEvent OnStartAttack,OnEndAttack;

    public void CallOnStartHit()
    {
        InventoryManager.SetObjectsColliders(true);

        OnStartAttack.Invoke();
    }

    public void CallOnEndHit()
    {
        InventoryManager.SetObjectsColliders(false);

        OnEndAttack.Invoke();
    }
}
