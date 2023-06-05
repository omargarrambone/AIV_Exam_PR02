using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class FluteUIScript : MonoBehaviour
{
    public Transform ArrowsUIParent,ArrowsPrefab;
    public TMPro.TMP_Text debugText;
    public int currentArrowIndex;
    public int arrowsToGenerate;
    public System.Tuple<int,FluteArrow, Vector2>[] fluteArrows;

    public UnityEvent OnCompleted, OnFail;

    void Awake()
    {
        fluteArrows = new System.Tuple<int, FluteArrow, Vector2>[arrowsToGenerate];

        for (int i = 0; i < fluteArrows.Length; i++)
        {
            Instantiate(ArrowsPrefab, ArrowsUIParent);
        }
    }

    private void OnEnable()
    {
        StartMinigame();
    }

    [ContextMenu("StartMinigame")]
    void StartMinigame()
    {
        currentArrowIndex = 0;
        SetRandomArrows();
        SetDebugText();
    }

    public void FluteKeysPress(InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf) return;

        if (context.performed)
        {
            Vector2 value = context.ReadValue<Vector2>();

            if(value == fluteArrows[currentArrowIndex].Item3)
            {
                ArrowsUIParent.GetChild(currentArrowIndex).gameObject.SetActive(false);
                currentArrowIndex++;
                SetDebugText();

                if (currentArrowIndex >= fluteArrows.Length)
                {
                    OnCompleted.Invoke();
                    Debug.Log("Completato!");
                    gameObject.SetActive(false);
                }
            }
            else
            {
                OnFail.Invoke();
                gameObject.SetActive(false);
                Debug.Log("Fallito! (Omar Garrambone)");
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

    void SetDebugText()
    {
        debugText.SetText("");
        for (int i = currentArrowIndex; i < fluteArrows.Length; i++)
        {
            debugText.text += fluteArrows[i] + " ";
        }
    }
}
