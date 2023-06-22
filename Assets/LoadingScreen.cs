using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingScreen : MonoBehaviour
{
    float timer, counter;
    int currentIndex;
    [SerializeField] TMPro.TMP_Text loadingText;

    string[] loadingTexts;

    private void Awake()
    {
        timer = 0.1f;
        counter = timer;
        loadingTexts = new string[] { "Loading.", "Loading..", "Loading...", "Loading...." };
    }

    private void OnEnable()
    {
        loadingText.SetText(loadingTexts[Random.Range(0,loadingTexts.Length)]);
    }

    void Update()
    {
        counter -= Time.deltaTime;

        if (counter < 0)
        {
            currentIndex++;
            currentIndex %= loadingTexts.Length;

            loadingText.SetText(loadingTexts[currentIndex]);

            counter = timer;
        }
    }
}
