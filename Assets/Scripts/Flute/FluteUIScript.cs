using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FluteUIScript : MonoBehaviour
{
    [SerializeField] private Transform ArrowsUIParent,ArrowsPrefab;
    [SerializeField] private TMPro.TMP_Text debugText;
    [SerializeField] private int arrowsToGenerate;
    [SerializeField] private UnityEvent OnCompleted, OnFail;
    [SerializeField] private PlayerInput playerInputScript;

    private float originalSpeed;
    private System.Tuple<int,FluteArrow, Vector2>[] fluteArrows;
    private int currentArrowIndex;

    void Awake()
    {
        fluteArrows = new System.Tuple<int, FluteArrow, Vector2>[arrowsToGenerate];

        for (int i = 0; i < fluteArrows.Length; i++)
        {
            Instantiate(ArrowsPrefab, ArrowsUIParent);
        }

        originalSpeed = playerInputScript.speed;

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartMinigame();
        playerInputScript.speed = 0;
    }

    private void OnDisable()
    {
        playerInputScript.speed = originalSpeed;
    }

    [ContextMenu("StartMinigame")]
    void StartMinigame()
    {
        currentArrowIndex = 0;
        SetRandomArrows();
    }

    public void FluteKeysPressCheck(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf) return;

        if (context.performed)
        {
            //to fix if a player keeps a key pressed (?)

            Vector2 value = context.ReadValue<Vector2>();

            if(value == fluteArrows[currentArrowIndex].Item3)
            {
                ArrowsUIParent.GetChild(currentArrowIndex).gameObject.SetActive(false);
                currentArrowIndex++;

                if (currentArrowIndex >= fluteArrows.Length)
                {
                    OnCompleted.Invoke();
                    gameObject.SetActive(false);
                }
            }
            else
            {
                OnFail.Invoke();
                gameObject.SetActive(false);
            }
        }

    }

    void SetRandomArrows()
    {
        for (int i = currentArrowIndex; i < fluteArrows.Length; i++)
        {
            FluteArrow index = (FluteArrow)Random.Range(0, (int)FluteArrow.LAST);

            Vector2 direction = Vector2.zero;

            switch (index)
            {
                case FluteArrow.Up:
                    direction = Vector2.up;
                    break;
                case FluteArrow.Down:
                    direction = Vector2.down;
                    break;
                case FluteArrow.Left:
                    direction = Vector2.left;
                    break;
                case FluteArrow.Right:
                    direction = Vector2.right;
                    break;
            }

            ArrowsUIParent.GetChild(i).localRotation = Quaternion.Euler(0, 0, ((int)index) * 90);
            ArrowsUIParent.GetChild(i).gameObject.SetActive(true);
            fluteArrows[i] = new System.Tuple<int, FluteArrow, Vector2>(i,index,direction);

        }
    }
}