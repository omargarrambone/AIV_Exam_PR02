using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintWeapon : MonoBehaviour
{
    static private TMPro.TMP_Text staticHintText;
    static private bool shouldShowText;

    [SerializeField] private TMPro.TMP_Text hintText;
    [SerializeField] private float timer, counter;

    private void Awake()
    {
        staticHintText = hintText;
        counter = timer;
    }

    private void HideHint()
    {
        hintText.SetText("");
    }

    static public void ShowHint(string hint)
    {
        shouldShowText = true;
        staticHintText.SetText(hint);
    }

    private void Update()
    {
        if (!shouldShowText) return;

        counter -= Time.deltaTime;

        if (counter < 0)
        {
            HideHint();
            counter = timer;
        }
    }
}
