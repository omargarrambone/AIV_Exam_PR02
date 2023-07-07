using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractableChest : MonoBehaviour
{
    // Start is called before the first frame update
    private bool isOpen = false;
    [SerializeField] private Animator _anim;

    public void OpenCloseChest()
    {
        isOpen = !isOpen;
        _anim.SetBool("isOpen", isOpen);
    }
}
