using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class DistanceMainMenu : MonoBehaviour
{
    public Transform Target;
    public Transform Camera;

    public float maxDistance;

    // Update is called once per frame
    void Update()
    {
        CheckDistance();
    }

    void CheckDistance()
    {
        if (Vector3.Distance(Camera.position, Target.position) < maxDistance)
        {
            if (SaveDataJSON.DoesSavedDataExist())
            {
                SceneManager.LoadScene("DontDestroyScene");
            }
            else
            {
                SceneManager.LoadScene("DontDestroyScene");
            }
            
        }
    }


}
