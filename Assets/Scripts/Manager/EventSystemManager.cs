using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EventSystemManger : MonoBehaviour
{
    public EventSystem eventsystem;


    public void SwitchCurrentButton(GameObject currentButton)
    {
        eventsystem.SetSelectedGameObject(currentButton);
    }

}
