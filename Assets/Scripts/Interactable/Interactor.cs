using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interactor : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayerMask;
    [SerializeField] private float maxDistance;
    private RaycastHit hit;
    [SerializeField] private Image interactImage;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color interactColor;

    Interactable actualInteractable;

    void Update()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, maxDistance, interactableLayerMask))
        {
            Interactable interactable = hit.collider.GetComponent<Interactable>();

            if (interactable != null)
            {
                interactImage.color = interactColor;
                actualInteractable = interactable;
            }

            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward*maxDistance, Color.green);
        }
        else
        {
            interactImage.color = defaultColor;
            actualInteractable = null;
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward*maxDistance, Color.red);
        }

    }

    public void Interact(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if(context.performed)
            actualInteractable.onInteract.Invoke();
    }

}
