using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

[System.Serializable]
public struct InventorySlot
{
    public ItemType ItemType;
    public int Value;
}

public class InventoryManager : MonoBehaviour
{
    static public InventorySlot[] InventoryItems { get; private set; }
    static private Image[] staticsInventoryImages;
    static private GameObject[] staticsInventoryGameObjects;
    static public int CurrentSlotIndex { get; private set; }
    static private Animator playerAnimator;

    [SerializeField] private GameObject[] inventoryGameObjects;
    [SerializeField] private Image[] inventoryImages;

    static private UnityEvent<ItemType> OnChangeItem;
    [SerializeField] private AnimatorLayerLerper animatorLayerLerper;

    void Start()
    {
        InventoryItems = new InventorySlot[5];

        for (int i = 0; i < InventoryItems.Length; i++)
        {
            InventoryItems[i].ItemType = (ItemType)i;
            InventoryItems[i].Value = 0;
        }

        InventoryItems[((int)ItemType.Fists)].Value = 1;
        InventoryItems[((int)ItemType.SmallKatana)].Value = 1;
        InventoryItems[((int)ItemType.Flute)].Value = 1;

        staticsInventoryImages = inventoryImages;

        foreach (Image image in staticsInventoryImages)
        {
            image.gameObject.SetActive(false);
        }

        CurrentSlotIndex = 0;

        staticsInventoryImages[CurrentSlotIndex].gameObject.SetActive(true);
        staticsInventoryGameObjects = inventoryGameObjects;

        playerAnimator = PlayerManager.PlayerGameObject.GetComponent<Animator>();

        OnChangeItem = new UnityEvent<ItemType>();
        OnChangeItem.AddListener(SetAnimatorLayers);
    }

    public static void AddItem(ItemType item)
    {
        InventoryItems[((int)item)].Value = 1;

        SetActualItem(item);

        //Add change sound();
    }

    public static void SetInventory(InventorySlot[] inventorySlots)
    {
        InventoryItems = inventorySlots;
    }

    public void SwapItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();

            staticsInventoryImages[CurrentSlotIndex].gameObject.SetActive(false);

            int newSlotIndex = CurrentSlotIndex;

            for (int i = 0; i < InventoryItems.Length; i++)
            {
                newSlotIndex += ((int)value);
                newSlotIndex %= ((int)ItemType.LAST);
                if (newSlotIndex < 0) newSlotIndex = ((int)ItemType.LAST - 1);

                if (InventoryItems[newSlotIndex].Value > 0)
                {
                    CurrentSlotIndex = newSlotIndex;
                    break;
                }
            }

            SetActualItem((ItemType)CurrentSlotIndex);
        }
    }

    static public void SetActualItem(int type)
    {
        SetActualItem((ItemType)type);
    }

    static public void SetActualItem(ItemType type)
    {
        for (int i = 0; i < ((int)ItemType.LAST); i++)
        {
            staticsInventoryGameObjects[i].SetActive(false);
            staticsInventoryImages[i].gameObject.SetActive(false);
        }

        staticsInventoryGameObjects[(int)type].gameObject.SetActive(true);

        ChangeImage((int)type);
        OnChangeItem.Invoke(type);        
    }

    private void SetAnimatorLayers(ItemType type)
    {
        StopAllCoroutines();

        switch (type)
        {
            case ItemType.Fists:
                animatorLayerLerper.StartLerp(1, 0);
                break;
            case ItemType.SmallKatana:
                animatorLayerLerper.StartLerp(1, 0);
                break;
            case ItemType.LongKatana:
                animatorLayerLerper.StartLerp(1, 1);
                break;
            case ItemType.Rod:
                animatorLayerLerper.StartLerp(1, 1);
                break;
            case ItemType.Flute:
                animatorLayerLerper.StartLerp(1, 0);
                break;
        }
    }

    static void ChangeImage(int newIndex)
    {
        staticsInventoryImages[CurrentSlotIndex].gameObject.SetActive(false);
        staticsInventoryImages[newIndex].gameObject.SetActive(true);
        CurrentSlotIndex = ((int)newIndex);
    }
}
