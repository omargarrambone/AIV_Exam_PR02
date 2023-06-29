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
    [SerializeField] private WeaponsManager weaponsManager;
    [SerializeField] private Transform ArrowsUIParent,ArrowsPrefab;
    [SerializeField] private PlayerInput playerInputScript;
    [SerializeField] private Image musicSheet;
    [SerializeField] private int arrowsToGenerate, lastArrowsToGenerate;
    [Header("Events")]
    [SerializeField] private UnityEvent<FluteArrow> OnCorrectArrow;
    [SerializeField] private UnityEvent OnStart,OnCompleted, OnFail;

    void Awake()
    {
        OnStart.AddListener(StartMinigame);
        OnStart.AddListener(PlayerManager.DisablePlayerMovement);

        musicSheet.color = Color.red;

        gameObject.SetActive(false);
    }

    void GenerateArrows()
    {
        lastArrowsToGenerate = arrowsToGenerate;
        fluteArrows = new System.Tuple<int, FluteArrow, Vector2>[arrowsToGenerate];

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
        }

    }

    private void OnEnable()
    {
        if(lastArrowsToGenerate != arrowsToGenerate)
        {
            GenerateArrows();
        }

        lastWeaponIndex = weaponsManager.CurrentSlotIndex;

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

        if (context.performed)
        {
            //TODO: to fix if a player keeps a key pressed (?)

            Vector2 value = context.ReadValue<Vector2>();

           //Debug.Log(value + " == " + fluteArrows[currentArrowIndex].Item3);

            if(value == fluteArrows[currentArrowIndex].Item3)
            {
                ArrowsUIParent.GetChild(currentArrowIndex).gameObject.SetActive(false);
                OnCorrectArrow.Invoke(fluteArrows[currentArrowIndex].Item2);
                musicSheet.color = currentArrowIndex % 2 != 0 ? Color.red : Color.blue;

                currentArrowIndex++;

                if (currentArrowIndex >= fluteArrows.Length)
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
        weaponsManager.SetActualItem(lastWeaponIndex);
        gameObject.SetActive(false);

        PlayerManager.EnablePlayerMovement();
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

            Vector3 rotation = new Vector3(0, 0, ((int)index) * 90);

            ArrowsUIParent.GetChild(i).localEulerAngles = rotation;

            Debug.Log(rotation + "==" + ArrowsUIParent.GetChild(i).localEulerAngles);

            ArrowsUIParent.GetChild(i).gameObject.SetActive(true);
            fluteArrows[i] = new System.Tuple<int, FluteArrow, Vector2>(i,index,direction);
        }
    }
}
