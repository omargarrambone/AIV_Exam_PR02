using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInOut : MonoBehaviour
{
    public Image blackScreen;
    public static bool FadeinOut = false;
    public bool AIncreasing = false;
    public float Opacity = 0f;
    public float ChangeSpeed = 0.1f;
    private void Update()
    {
        if (FadeinOut)
        {
            FadeinOut = false;
            AIncreasing = true;
        }
        if (Opacity < 1f && AIncreasing)
        {
            Opacity += ChangeSpeed;
        }
        else
        {
            if (AIncreasing)
            {
                AIncreasing = false;
            }
        }
        if (!AIncreasing && Opacity > 0f)
        {
            Opacity -= ChangeSpeed;
        }
        blackScreen.color = new Color(0f, 0f, 0f, Opacity);
    }
}
