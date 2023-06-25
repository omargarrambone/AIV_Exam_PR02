using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    public GameObject StartPosition;
    public GameObject OptionPosition;

    public Canvas OptionMenu;
    public Canvas StartMenu;

    Transform ObjPosition;

    public Animator _anim;

    [SerializeField]
    float Speed;

    bool Transition = false;

    void Start()
    {
        transform.position = StartPosition.transform.position;
    }

    private void Update()
    {
        if (Transition)
        {
            transform.position = Vector3.Lerp(transform.position, ObjPosition.position, Speed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, ObjPosition.rotation, Speed * Time.deltaTime);
        }
    }

    public void ButtonPressed(GameObject TargetPos)
    {
        ObjPosition = TargetPos.transform;
        Transition = true;
    }

    public void StartPlay()
    {
        _anim.SetBool("StartPlay", true);
    }


    //public void LookAtCanvas(Canvas cavasPos)
    //{
    //    transform.LookAt(cavasPos.transform);
    //}

}
