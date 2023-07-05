using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image blackScreen;
    public static bool FadeinOut = false;
    public bool AIincreasing = false;
    public float Opacity = 0f;
    public float ChangeSpeed = 0.001f;
    private void Update()
    {
        if (FadeinOut)
        {
            FadeinOut = false;
            AIincreasing = true;
        }
        if (Opacity<1f&&AIincreasing)
        {
            Opacity += ChangeSpeed;
        }
        else
        {
            if (AIincreasing)
            {
                AIincreasing = false;
            }
        }
        if (!AIincreasing&&Opacity>0f)
        {
            Opacity -= ChangeSpeed;
        }
        blackScreen.color = new Color(0f, 0f, 0f, Opacity);
    }
}
