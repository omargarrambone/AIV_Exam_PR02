using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FluteUIScript : MonoBehaviour
{
    [SerializeField] private Transform ArrowsUIParent,ArrowsPrefab;
    [SerializeField] private int arrowsToGenerate;
    [SerializeField] private UnityEvent OnStart,OnCompleted, OnFail;
    [SerializeField] private UnityEvent<FluteArrow> OnCorrectArrow;
    [SerializeField] private PlayerInput playerInputScript;
    private float originalSpeed;
    private System.Tuple<int,FluteArrow, Vector2>[] fluteArrows;
    private int currentArrowIndex;

    void Awake()
    {
        fluteArrows = new System.Tuple<int, FluteArrow, Vector2>[arrowsToGenerate];

        for (int i = 0; i < fluteArrows.Length; i++)
        {
            Transform arrow = Instantiate(ArrowsPrefab, ArrowsUIParent);
            Image image = arrow.GetComponent<Image>();
            image.color = i%2 == 0 ? Color.red : Color.blue;
        }

        originalSpeed = playerInputScript.speed;
        Rigidbody playerRb = playerInputScript.gameObject.GetComponent<Rigidbody>();
        playerRb.velocity = new Vector3(0, playerRb.velocity.y, 0);

        gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        StartMinigame();
        playerInputScript.speed = 0;
        OnStart.Invoke();
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
                OnCorrectArrow.Invoke(fluteArrows[currentArrowIndex].Item2);
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
