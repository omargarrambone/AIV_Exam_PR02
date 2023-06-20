using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    public static CharacterController _charactercontroller;
    void Awake()
    {
       if(PlayerGameObject == null) PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
    }

    static public void SetPosition(Vector3 newPosition)
    {
        PlayerGameObject.transform.position = newPosition;
    }

    static public void SetPosition(int x, int y, int z)
    {
        PlayerGameObject.transform.position = new Vector3(x, y, z);
    }

    static public void SetRotation(Vector3 newRotation)
    {
        PlayerGameObject.transform.rotation = Quaternion.Euler(newRotation);
    }

    static public void SetRotation(Quaternion newRotation)
    {
        PlayerGameObject.transform.rotation = newRotation;
    }

    public void Check()
    {
        if (PlayerGameObject.GetComponent<HealthManager>().IsDead == true)
        {
            StartCoroutine(WaitForDeathAnimation());
        }
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("GameOverScene");
        yield return new WaitForSeconds(2);
        PlayerGameObject.GetComponent<HealthManager>().ResetHealt();
    }
}
