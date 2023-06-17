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
    public float BloodDamage;
    public float StunDamage;
    public bool IsTaken;
}

public class InventoryManager : MonoBehaviour
{
    static public InventorySlot[] InventoryItems { get; private set; }
    static private Image[] staticsInventoryImages;
    static private GameObject[] staticsInventoryGameObjects;
    static public int CurrentSlotIndex { get; private set; }

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
        }

        InventoryItems[((int)ItemType.Fists)].IsTaken = true;
        InventoryItems[((int)ItemType.SmallKatana)].IsTaken = true;
        InventoryItems[((int)ItemType.LongKatana)].IsTaken = false;
        InventoryItems[((int)ItemType.Rod)].IsTaken = false;
        InventoryItems[((int)ItemType.Flute)].IsTaken = true;

        staticsInventoryImages = inventoryImages;

        foreach (Image image in staticsInventoryImages)
        {
            image.gameObject.SetActive(false);
        }

        CurrentSlotIndex = 0;

        staticsInventoryImages[CurrentSlotIndex].gameObject.SetActive(true);
        staticsInventoryGameObjects = inventoryGameObjects;

        OnChangeItem = new UnityEvent<ItemType>();
        OnChangeItem.AddListener(SetAnimatorLayers);

        SetWeaponsDamages();
    }

    void SetWeaponsDamages()
    {
        InventoryItems[((int)ItemType.Fists)].BloodDamage = 1f;
        InventoryItems[((int)ItemType.Fists)].StunDamage = 10f;

        InventoryItems[((int)ItemType.SmallKatana)].BloodDamage = 10f;
        InventoryItems[((int)ItemType.SmallKatana)].StunDamage = 20f;

        InventoryItems[((int)ItemType.LongKatana)].BloodDamage = 15f;
        InventoryItems[((int)ItemType.LongKatana)].StunDamage = 20f;

        InventoryItems[((int)ItemType.Rod)].BloodDamage = 5f;
        InventoryItems[((int)ItemType.Rod)].StunDamage = 30f;

        InventoryItems[((int)ItemType.Flute)].BloodDamage = 0f;
        InventoryItems[((int)ItemType.Flute)].StunDamage = 0f;
    }

    public static void AddItem(ItemType item)
    {
        InventoryItems[((int)item)].IsTaken = true;

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

                if (InventoryItems[newSlotIndex].IsTaken == true)
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
        EnemyDamageManager.ChangeDamage(InventoryItems[((int)type)].BloodDamage, InventoryItems[((int)type)].StunDamage);
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
