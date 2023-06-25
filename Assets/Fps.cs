using UnityEngine;
using System.Collections;

public class Fps : MonoBehaviour
{
    private float count;
    [SerializeField] private TMPro.TMP_Text text;

    private IEnumerator Start()
    {
        GUI.depth = 2;
        while (true)
        {
            count = 1f / Time.unscaledDeltaTime;
            text.SetText("FPS: " + Mathf.Round(count));
            yield return new WaitForSeconds(0.1f);
        }
    }
}