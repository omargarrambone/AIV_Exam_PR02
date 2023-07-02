using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RythmKeysToPlayer : MonoBehaviour
{
    private UnityEngine.InputSystem.PlayerInput playerInput;
    [SerializeField] private ArrowsCheckHit[] arrowsCheckHits;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = PlayerManager.PlayerGameObject.GetComponent<UnityEngine.InputSystem.PlayerInput>();

        AddListeners();
    }


    public void AddListeners()
    {
        for (int i = 0; i < arrowsCheckHits.Length; i++)
        {
            playerInput.actionEvents[0].AddListener(arrowsCheckHits[i].CheckKeyPressed);
        }
    }

    public void RemoveListeners()
    {
        for (int i = 0; i < arrowsCheckHits.Length; i++)
        {
            playerInput.actionEvents[0].RemoveListener(arrowsCheckHits[i].CheckKeyPressed);

        }
    }
}
