using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngineInternal;
public class BugPlayerInput : MonoBehaviour
{
    Rigidbody _rb;
    PlayerInput playerInput;
    Movement Movement;
    Animator _anim;

    public bool IsGrounded;
    public bool IsAttacking;
    float turnSmooth = 0.1f;
    float turnSmoothVelocity;

    public int jumpCount = 0;

    public float speed;
    [SerializeField]
    float JumpHeight;

    [SerializeField] private float animationLightAttackFinishTime = 0.5f;
    [SerializeField] private float animationHeavyAttackFinishTime = 0.5f;
    private float Height = 0.1f;
    private bool isRunning = false;
    private bool isAttacking = false;
    private bool isHeavyAttacking = false;
    private bool isAttackingGoing = false;
    private bool isHeavyAttackingGoing = false;

    private Vector2 inputVector;

    [Header("Dash Variables")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Header("UI")]
    public GameObject Panel;

    // Start is called before the first frame update
    void Awake()
    {
        _anim = GetComponent<Animator>();
        _rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Movement = new Movement();

        Movement.Player.Enable();
    }

    void FixedUpdate()
    {
        inputVector = Movement.Player.Movement.ReadValue<Vector2>();
        ChangeDirection(inputVector);
        _rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
        CheckIsGrounded();
    }

    private void Update()
    {
        _anim.SetFloat("Velocity", inputVector.sqrMagnitude);
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            _anim.SetTrigger("IsAttacking");
        }

    }

    public void ChangeDirection(Vector2 input)
    {
        Vector2 dir = input.normalized;

        if (dir.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            float angle = Mathf.SmoothDampAngle(transform.localEulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmooth);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    public void Pause(InputAction.CallbackContext context)
    {
        //if (context.performed && Panel.gameObject.activeSelf == false)
        //{
        //    Panel.gameObject.SetActive(true);
        //    Debug.Log("pause");
        //}
        //else if (context.performed && Panel.gameObject.activeSelf == true)
        //{
        //    Panel.gameObject.SetActive(false);
        if (context.performed)
        {
            if (Panel.gameObject.activeSelf == false)
            {
                Panel.gameObject.SetActive(true);
            }
            else
            {
                Panel.gameObject.SetActive(false);
            }
        }
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpCount++;
            _anim.SetInteger("JumpCount", jumpCount);
            if (jumpCount < 2)
            {
                _rb.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
            }
        }

    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            float interactRange = 2f;
            Collider[] colliderArray = Physics.OverlapSphere(transform.position, interactRange);
            foreach (Collider collider in colliderArray)
            {
                if (collider.TryGetComponent(out NPCInteractable npcInteractable))
                {
                    npcInteractable.Interact();
                }
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            StartCoroutine(Dash());
        }
    }


    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        _rb.velocity = transform.forward * dashingPower;
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }

    void CheckIsGrounded()
    {
        if (Physics.Raycast(transform.position, Vector3.down, Height))
        {
            IsGrounded = true;
            jumpCount = 0;
            _anim.SetBool("IsGrounded", IsGrounded);
            _anim.SetInteger("JumpCount", jumpCount);
            Debug.DrawRay(transform.position, Vector3.down * Height, UnityEngine.Color.green);
        }
        else
        {
            IsGrounded = false;
            Debug.DrawRay(transform.position, Vector3.down * Height, UnityEngine.Color.red);
            _anim.SetBool("IsGrounded", IsGrounded);
        }
    }
}