using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackScript : MonoBehaviour
{
    public UnityEvent OnStartAttack,OnEndAttack;
    public WeaponsManager weaponsManager;

    public void CallOnStartHit()
    {
        weaponsManager.SetObjectsColliders(true);

        OnStartAttack.Invoke();
    }

    public void CallOnEndHit()
    {
        weaponsManager.SetObjectsColliders(false);

        OnEndAttack.Invoke();
    }
}
