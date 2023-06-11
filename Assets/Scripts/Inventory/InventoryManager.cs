using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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
    [SerializeField] private Image[] inventoryImages;

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

        staticsInventoryImages[0].gameObject.SetActive(true);
        CurrentSlotIndex = 0;

        staticsInventoryGameObjects = new GameObject[inventoryImages.Length];
    }

    public static void AddItem(ItemType item)
    {
        InventoryItems[((int)item)].Value = 1;

        ChangeImage((int)item);

        CurrentSlotIndex = ((int)item);

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

            ChangeImage(CurrentSlotIndex);
        }
    }

    static public void SetActualItem(ItemType type)
    {
        foreach (Image image in staticsInventoryImages)
        {
            image.gameObject.SetActive(false);
        }

        staticsInventoryImages[((int)type)].gameObject.SetActive(true);
        CurrentSlotIndex = ((int)type);
    }

    static void ChangeImage(int newIndex)
    {
        staticsInventoryImages[CurrentSlotIndex].gameObject.SetActive(false);
        staticsInventoryImages[newIndex].gameObject.SetActive(true);
    }

    public void LoadInventory()
    {
        SaveData data = SaveDataJSON.SavedData;

        SetInventory(data.playerData.inventoryItems);

        SetActualItem((ItemType)data.playerData.currentWeapon);
    }
}
