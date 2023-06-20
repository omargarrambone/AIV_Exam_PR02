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
    private Animator _anim;

    void Start()
    {
        _anim = GetComponent<Animator>();
    }

    public void OpenCloseChest()
    {
        if (isOpen == false)
        {
            isOpen = true;
            _anim.SetBool("isOpen", isOpen);
        }
        else if (isOpen == true)
        {          
            _anim.SetBool("isOpen", isOpen);
        }
    }
}
