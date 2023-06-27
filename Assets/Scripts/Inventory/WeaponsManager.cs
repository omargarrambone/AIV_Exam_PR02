using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class WeaponsManager : MonoBehaviour
{
    [SerializeField] private Weapon[] Weapons;
    public bool[] TakenWeapons { get; private set; }
    public int CurrentSlotIndex { get; private set; }

    [SerializeField] private GameObject[] inventoryGameObjects;
    [SerializeField] private Image[] inventoryImages;
    private Collider[] weaponsColliders;

    [Header("Animation Layers")]
    [SerializeField] private AnimatorLayerLerper animatorLayerLerper1;
    [SerializeField] private AnimatorLayerLerper animatorLayerLerper2;
    [SerializeField] private AnimatorLayerLerper animatorLayerLerper3;

    void Start()
    {
        TakenWeapons = new bool[5];

        TakenWeapons[((int)ItemType.Fists)] = true;
        TakenWeapons[((int)ItemType.SmallKatana)] = true;
        TakenWeapons[((int)ItemType.LongKatana)] = false;
        TakenWeapons[((int)ItemType.Rod)] = false;
        TakenWeapons[((int)ItemType.Flute)] = true;

        for (int i = 0; i < inventoryImages.Length; i++)
        {
            inventoryImages[i].sprite = Weapons[i].DisplayImage;
            inventoryImages[i].gameObject.SetActive(false);
        }

        inventoryImages[CurrentSlotIndex].gameObject.SetActive(true);
        SetAnimatorLayers((ItemType)CurrentSlotIndex);

        weaponsColliders = new Collider[inventoryGameObjects.Length-1]; // no flute

        int lastColliderIndex=0;

        for (int i = 0; i < inventoryGameObjects.Length; i++)
        {
            Collider collider = inventoryGameObjects[i].GetComponent<Collider>();

            if (collider == null) continue;

            weaponsColliders[lastColliderIndex] = collider;
            weaponsColliders[lastColliderIndex].enabled = false;
            lastColliderIndex++;
        }

        SetActualItem(CurrentSlotIndex);
    }

    public void AddItem(ItemType item)
    {
        TakenWeapons[((int)item)] = true;

        SetActualItem(item);

        //Add change sound();
    }

    public void SetInventory(bool[] takenWeapons)
    {
        TakenWeapons = takenWeapons;
    }

    public void SwapItem(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float value = context.ReadValue<float>();

            inventoryImages[CurrentSlotIndex].gameObject.SetActive(false);

            int newSlotIndex = CurrentSlotIndex;

            for (int i = 0; i < TakenWeapons.Length; i++)
            {
                newSlotIndex += ((int)value);
                newSlotIndex %= ((int)ItemType.LAST);
                if (newSlotIndex < 0) newSlotIndex = ((int)ItemType.LAST - 1);

                if (TakenWeapons[newSlotIndex] == true)
                {
                    CurrentSlotIndex = newSlotIndex;
                    break;
                }
            }

            SetActualItem((ItemType)CurrentSlotIndex);
        }
    }

    public void SetActualItem(int type)
    {
        SetActualItem((ItemType)type);
    }

    public void SetActualItem(ItemType type)
    {
        for (int i = 0; i < ((int)ItemType.LAST); i++)
        {
            inventoryGameObjects[i].SetActive(false);
            inventoryImages[i].gameObject.SetActive(false);
        }

        inventoryGameObjects[(int)type].gameObject.SetActive(true);

        ChangeImage((int)type);
        EnemyDamageManager.ChangeDamage(Weapons[((int)type)].BloodDamage, Weapons[((int)type)].StunDamage);
        SetAnimatorLayers(type);
    }

    private void SetAnimatorLayers(ItemType type)
    {
        StopAllCoroutines();

        switch (type)
        {
            case ItemType.Fists:
                animatorLayerLerper1.StartLerp(1, 0);
                animatorLayerLerper2.StartLerp(2, 0);
                animatorLayerLerper3.StartLerp(3, 1);
                break;

            case ItemType.SmallKatana:
                animatorLayerLerper1.StartLerp(1, 0);
                animatorLayerLerper2.StartLerp(2, 1);
                animatorLayerLerper3.StartLerp(3, 0);
                break;

            case ItemType.LongKatana:
                animatorLayerLerper1.StartLerp(1, 1);
                animatorLayerLerper2.StartLerp(2, 0);
                animatorLayerLerper3.StartLerp(3, 0);
                break;

            case ItemType.Rod:
                animatorLayerLerper1.StartLerp(1, 1);
                animatorLayerLerper2.StartLerp(2, 0);
                animatorLayerLerper3.StartLerp(3, 0);
                break;

            case ItemType.Flute:
                animatorLayerLerper1.StartLerp(1, 0);
                animatorLayerLerper2.StartLerp(2, 1);
                animatorLayerLerper3.StartLerp(3, 0);
                break;
        }
    }

    void ChangeImage(int newIndex)
    {
        inventoryImages[CurrentSlotIndex].gameObject.SetActive(false);
        inventoryImages[newIndex].gameObject.SetActive(true);
        CurrentSlotIndex = ((int)newIndex);
    }

    public void SetObjectsColliders(bool value)
    {
        for (int i = 0; i < weaponsColliders.Length; i++)
        {
            weaponsColliders[i].GetComponent<Collider>().enabled = value;
        }
    }
}
