using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoodOrBadEdTrigger : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private AudioSource bgMusic;

    void OnEnable()
    {
        StartCoroutine(FinishCutscene());
    }

    // Update is called once per frame
    IEnumerator FinishCutscene()
    {
        yield return new WaitForSeconds(7);
        PlayerManager.SetPosition(Vector3.zero);
        Destroy(PlayerManager.PlayerGameObject.transform.parent.gameObject);
        bgMusic.Stop();
        if (NPCManager.PurifiedEnemies <= 5)
        {
            SceneManager.LoadScene("BadEdCaltanissetta", LoadSceneMode.Single);
        }
        else
        {
            SceneManager.LoadScene("GoodEdCaltanissetta", LoadSceneMode.Single);
        }
        
    }
}
