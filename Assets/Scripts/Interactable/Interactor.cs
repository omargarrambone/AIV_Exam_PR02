using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Interactor : MonoBehaviour
{
    [SerializeField] private Interactable actualInteractable;
    [SerializeField] private TMPro.TMP_Text guiText;

    public void Interact(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if(actualInteractable is not null)
            {
                actualInteractable.OnInteract.Invoke();
                guiText.SetText("");
            }
        }
    }

    private void OnTriggerStay(Collider other)
    {
        actualInteractable = other.GetComponent<Interactable>();
        guiText.SetText($"Take [{actualInteractable.ItemName}]");
        //fai comparire scritta premi E per interagire ed altro

    }

    private void OnTriggerExit(Collider other)
    {
        actualInteractable = null;
        guiText.SetText("");
    }
}
