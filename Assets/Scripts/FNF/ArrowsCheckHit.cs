using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



public class ArrowsCheckHit : MonoBehaviour
{
    [SerializeField] GameObject actualArrow;
    [SerializeField] FluteArrow myDirection;
    [SerializeField] Vector2 myVectorDirection;
    static public UnityEvent<float> OnHittedNote;
    static public UnityEvent OnMissedNote;

    void Awake()
    {
        switch (myDirection)
        {
            case FluteArrow.Down:
                myVectorDirection = Vector2.down;
                break;
            case FluteArrow.Right:
                myVectorDirection = Vector2.right;
                break;
            case FluteArrow.Up:
                myVectorDirection = Vector2.up;
                break;
            case FluteArrow.Left:
                myVectorDirection = Vector2.left;
                break;
        }

        OnHittedNote = new UnityEvent<float>();
        OnMissedNote = new UnityEvent();
    }

    public void CheckKeyPressed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (actualArrow == null) return;

            Vector2 value = context.ReadValue<Vector2>();

            if(value == myVectorDirection)
            {
                float distance = Vector2.Distance(transform.position, actualArrow.transform.position);

                Destroy(actualArrow);
                OnHittedNote.Invoke(distance);
            }

        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (actualArrow) return;

        if (other.CompareTag("RythmArrow"))
        {
            actualArrow = other.gameObject;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        OnMissedNote.Invoke();
        Destroy(actualArrow, 2f);
        actualArrow = null;
    }

}
