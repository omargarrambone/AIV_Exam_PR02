using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShooter : MonoBehaviour
{
    public string FolderName;
    public string filename;

    [ContextMenu("ScrenSHotTAke")]
    void Screenshot()
    {
        ScreenCapture.CaptureScreenshot("Assets/Materials/Cubemaps/"+FolderName +"/screenshot " + filename + ".png");
        Debug.Log("A screenshot was taken!");
    }
}
