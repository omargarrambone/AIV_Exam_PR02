using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodOrBadEdTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource bgMusic;

    void Start()
    {
        StartCoroutine(FinishCutscene());
    }

    // Update is called once per frame
    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(7);
        bgMusic.Stop();
        if (SaveDataJSON.SavedData.worldData.enemiesPurified <= 5)
        {
            SceneManager.LoadScene("BadEdCaltanissetta", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("GoodEdCaltanissetta", LoadSceneMode.Single);
        }
        
    }
}
