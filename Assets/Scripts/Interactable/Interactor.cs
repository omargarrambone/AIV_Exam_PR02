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
            if(actualInteractable != null)
            {
                actualInteractable.OnInteract.Invoke();
                guiText.SetText("");
            }
        }
    }

    private void Update()
    {
        if (actualInteractable && actualInteractable.isActiveAndEnabled)
        {
            guiText.SetText($"Take [{actualInteractable.ItemName}]");
        }
        else
        {
            guiText.SetText("");
        }
    }

    private void OnTriggerStay(Collider other)
    {
        actualInteractable = other.GetComponent<Interactable>();
        if (actualInteractable == null) { return; }
    }

    private void OnTriggerExit(Collider other)
    {
        actualInteractable = null;
    }
}
