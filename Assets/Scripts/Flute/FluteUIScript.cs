using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class FluteUIScript : MonoBehaviour
{
    private System.Tuple<int,FluteArrow, Vector2>[] fluteArrows;
    [SerializeField] private int currentArrowIndex, lastWeaponIndex;

    [Header("References")]
    [SerializeField] private Transform ArrowsUIParent,ArrowsPrefab;
    [SerializeField] private PlayerInput playerInputScript;
    [SerializeField] private int arrowsToGenerate, maxArrows;

    [Header("Events")]
    public UnityEvent<FluteArrow> OnCorrectArrow;
    public UnityEvent OnStart,OnCompleted, OnFail;

    void Awake()
    {
        GenerateArrows();

        OnStart.AddListener(StartMinigame);
        OnStart.AddListener(PlayerManager.DisablePlayerMovement);
        OnCompleted.AddListener(PlayerManager.EnablePlayerMovement);


        gameObject.SetActive(false);
    }

    void GenerateArrows()
    {
        fluteArrows = new System.Tuple<int, FluteArrow, Vector2>[maxArrows];

        for (int i = 0; i < ArrowsUIParent.childCount; i++)
        {
            Destroy(ArrowsUIParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < fluteArrows.Length; i++)
        {
            Transform arrow = Instantiate(ArrowsPrefab, ArrowsUIParent);
            arrow.name = i + " arrow";
            Image image = arrow.GetComponent<Image>();
            image.color = i % 2 == 0 ? Color.red : Color.blue;
            arrow.gameObject.SetActive(false);
        }

    }

    private void OnEnable()
    {
        if (arrowsToGenerate > maxArrows) arrowsToGenerate = maxArrows;

        OnStart.Invoke();        
    }

    void StartMinigame()
    {
        currentArrowIndex = 0;
        SetRandomArrows();
    }

    public void FluteKeysPressCheck(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (!gameObject.activeSelf) return;

        if (context.canceled)
        {
            Vector2 value = context.ReadValue<Vector2>();

            if(value == fluteArrows[currentArrowIndex].Item3)
            {
                ArrowsUIParent.GetChild(currentArrowIndex).gameObject.SetActive(false);
                OnCorrectArrow.Invoke(fluteArrows[currentArrowIndex].Item2);
                currentArrowIndex++;

                if (currentArrowIndex >= arrowsToGenerate)
                {
                    OnCompleted.Invoke();
                    OnFinished();
                }
            }
            else
            {
                OnFail.Invoke();
                OnFinished();
            }
        }

    }

    void OnFinished()
    {
        PlayerManager.EnablePlayerMovement();

        for (int i = 0; i < ArrowsUIParent.childCount; i++)
        {
            ArrowsUIParent.GetChild(i).gameObject.SetActive(false);
        }

        gameObject.SetActive(false);

    }

    void SetRandomArrows()
    {
        for (int i = 0; i < arrowsToGenerate; i++)
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

            Vector3 rotation = new Vector3(0, 0, ((int)index) * 90);

            ArrowsUIParent.GetChild(i).localEulerAngles = rotation;

            ArrowsUIParent.GetChild(i).gameObject.SetActive(true);
            fluteArrows[i] = new System.Tuple<int, FluteArrow, Vector2>(i,index,direction);
        }
    }
}
