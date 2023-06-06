using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPurification : MonoBehaviour
{
    public bool IsStunned;
    public Transform PurificatedLocation;
    private Animator Animator;

    public void PurificatedAnimation()
    {
        Animator.SetBool("purificated", true);
    }
}
