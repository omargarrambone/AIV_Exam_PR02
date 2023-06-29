using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    public static CharacterController PlayerCharactercontroller;
    public static PlayerInput PlayerInput;
    public static CameraFollow CameraFollow;
    public static SceneMngr sceneMngr;
    public float minY;

    //[SerializeField] private Material invisibleWall;
    void Awake()
    {
       if(PlayerGameObject == null) PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
       if (PlayerCharactercontroller == null) PlayerCharactercontroller = PlayerGameObject.GetComponent<CharacterController>();
       if (PlayerInput == null) PlayerInput = PlayerGameObject.GetComponent<PlayerInput>();
       if (CameraFollow == null) CameraFollow = Camera.main.gameObject.GetComponent<CameraFollow>();

        Cursor.lockState = CursorLockMode.Locked;
    }

    static public void SetPosition(Vector3 newPosition)
    {
        PlayerCharactercontroller.enabled = false;
        PlayerGameObject.transform.position = newPosition;
        PlayerCharactercontroller.enabled = true;
    }

    static public void SetPosition(int x=0, int y=0, int z=0)
    {
        PlayerCharactercontroller.enabled = false;
        PlayerGameObject.transform.position = new Vector3(x, y, z);
        PlayerCharactercontroller.enabled = true;
    }

    static public void SetRotation(Vector3 newRotation)
    {
        PlayerCharactercontroller.enabled = false;
        PlayerGameObject.transform.rotation = Quaternion.Euler(newRotation);
        PlayerCharactercontroller.enabled = true;
    }

    static public void SetRotation(Quaternion newRotation)
    {
        PlayerCharactercontroller.enabled = false;
        PlayerGameObject.transform.rotation = newRotation;
        PlayerCharactercontroller.enabled = true;
    }

    public void Death()
    {
        StartCoroutine(WaitForDeathAnimation());
        DisablePlayerMovement();
    }

    IEnumerator WaitForDeathAnimation()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene("GameOverScene");
        SetPosition(new Vector3(41.6f, 19.8f, 11.9f));
        CameraFollow.ResetCameraTarget();
        yield return new WaitForSeconds(0.1f);
        PlayerGameObject.GetComponent<HealthManager>().ResetHealth();
    }

    private void Update()
    {
        if(PlayerGameObject.transform.position.y < minY)
        {
            SceneManager.LoadScene("GameOverScene");
            SetPosition(Vector3.zero);
            PlayerGameObject.GetComponent<HealthManager>().ResetHealth();
        }

        //invisibleWall.SetVector("_PlayerPosition", PlayerGameObject.transform.position);
        //Debug.Log(invisibleWall.GetVector("_PlayerPosition"));
    }

    static public void DisablePlayerMovement()
    {
        PlayerInput.enabled = false;
    }

    static public void EnablePlayerMovement()
    {
        PlayerInput.enabled = true;
    }

    static public void EnableDisablePlayerMovement(bool value)
    {
        PlayerInput.enabled = value;
    }
}
