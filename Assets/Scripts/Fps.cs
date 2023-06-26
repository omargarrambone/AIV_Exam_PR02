using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{
    private float count;
    [SerializeField] private TMPro.TMP_Text text;

    private void Update()
    {
        count = 1f / Time.unscaledDeltaTime;
        text.SetText("FPS: " + Mathf.Round(count));
    }
}