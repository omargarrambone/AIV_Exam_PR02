using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    static private Dictionary<ItemType, int> inventoryItems;
    static private Image[] staticsInventoryImages;
    private static int currentSlotIndex;
    [SerializeField] private Image[] inventoryImages;

    void Start()
    {
        inventoryItems = new Dictionary<ItemType, int>
        {
            { ItemType.Fists, 1 },
            { ItemType.SmallKatana, 1 },
            { ItemType.LongKatana, 0 },
            { ItemType.Rod, 0 },
            { ItemType.Flute, 0 },
        };

        staticsInventoryImages = inventoryImages;

        foreach (Image image in staticsInventoryImages)
        {
            image.gameObject.SetActive(false);
        }

        staticsInventoryImages[0].gameObject.SetActive(true);
        currentSlotIndex = 0;
    }

    public static void AddItem(ItemType item)
    {
        inventoryItems[item] = 1;

        currentSlotIndex = ((int)item);
        ChangeImage((int)item);

        //Add change sound();
    }

    public void SwapItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();

            staticsInventoryImages[currentSlotIndex].gameObject.SetActive(false);

            int newSlotIndex = currentSlotIndex;

                for (int i = 0; i < inventoryItems.Count; i++)
                {
                    newSlotIndex += ((int)value);
                    newSlotIndex %= ((int)ItemType.LAST);
                    if (newSlotIndex < 0) newSlotIndex = ((int)ItemType.LAST - 1);

                    if (inventoryItems[(ItemType)newSlotIndex] > 0)
                    {
                        currentSlotIndex = newSlotIndex;
                        break;
                    }
                }

            ChangeImage(currentSlotIndex);
        }
    }

    static void ChangeImage(int newIndex)
    {
        staticsInventoryImages[currentSlotIndex].gameObject.SetActive(false);
        staticsInventoryImages[newIndex].gameObject.SetActive(true);
    }

    public static bool HasItem(ItemType item)
    {
        if (inventoryItems.ContainsKey(item))
        {
            return true;
        }

        return false;
    }
}
