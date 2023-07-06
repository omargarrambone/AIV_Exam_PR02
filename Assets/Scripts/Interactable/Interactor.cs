using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Interactable actualInteractable;
    [SerializeField] private TMPro.TMP_Text guiText;
    [SerializeField] private float radius, maxDistance;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private Collider[] colliders;

    public void Interact(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(actualInteractable != null)
            {
                actualInteractable.OnInteract.Invoke();
            }
        }
    }

    private void Update()
    {
        if (actualInteractable && actualInteractable.isActiveAndEnabled)
        {
            guiText.SetText($"{actualInteractable.ItemName}");
        }
        else
        {
            guiText.SetText("");
        }

        colliders = Physics.OverlapSphere(transform.position, radius, layerMask);

        if (colliders.Length > 0)
        {
            actualInteractable = colliders[0].GetComponent<Interactable>();
        }
        else if(actualInteractable!= null)
        {
            //actualInteractable.OnPlayerExitRange.Invoke();
            actualInteractable = null;
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
