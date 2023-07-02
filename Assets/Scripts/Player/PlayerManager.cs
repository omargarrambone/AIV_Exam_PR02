using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    public static GameObject PlayerGameObject { get; private set; }
    private static Animator playerAnimator { get; set; }
    public static CharacterController PlayerCharactercontroller;
    public static PlayerInput PlayerInput;
    public static CameraFollow CameraFollow;
    public static SceneMngr sceneMngr;

    static private float deathTimer, deathCounter;
    static private bool isDying;

    public float minY;

    //[SerializeField] private Material invisibleWall;
    void Awake()
    {
       if(PlayerGameObject == null) PlayerGameObject = GameObject.FindGameObjectWithTag("Player");
       if (PlayerCharactercontroller == null) PlayerCharactercontroller = PlayerGameObject.GetComponent<CharacterController>();
       if (PlayerInput == null) PlayerInput = PlayerGameObject.GetComponent<PlayerInput>();
       if (CameraFollow == null) CameraFollow = Camera.main.gameObject.GetComponent<CameraFollow>();
       if (playerAnimator == null) playerAnimator = PlayerGameObject.GetComponent<Animator>();

        Cursor.lockState = CursorLockMode.Locked;

        deathTimer = 3f;
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

    public static void Death()
    {
        if (isDying) return;

        deathCounter = deathTimer;
        isDying = true;
        DisablePlayerMovement();
        GameManager.GameState = GameState.Paused;

        SetTriggerAnimation("Death");
    }

    public static void SetTriggerAnimation(string triggerName)
    {
        playerAnimator.SetTrigger(triggerName);
    }

    private void Update()
    {
        if(PlayerGameObject.transform.position.y < minY)
        {
            SceneManager.LoadScene("GameOverScene");
            SetPosition(Vector3.zero);
            PlayerGameObject.GetComponent<HealthManager>().ResetHealth();
        }

        if (isDying)
        {
            deathCounter -= Time.deltaTime;

            if (deathCounter < 0)
            {
                SceneManager.LoadScene("GameOverScene");
                SetPosition(new Vector3(41.6f, 19.8f, 11.9f));
                CameraFollow.ResetCameraTarget();
                PlayerGameObject.GetComponent<HealthManager>().ResetHealth();
                isDying = false;
            }
        }
    }

    static public void DisablePlayerMovement()
    {
        PlayerInput.enabled = false;
        PlayerInput.ShouldNotMove = true;
    }

    static public void EnablePlayerMovement()
    {
        PlayerInput.enabled = true;
        PlayerInput.ShouldNotMove = false;
    }

    static public void EnableDisablePlayerMovement(bool value)
    {
        if (value) EnablePlayerMovement();
        else DisablePlayerMovement();
    }
}
