using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngineInternal;

public class PlayerInput : MonoBehaviour
{
    public Rigidbody _rb;
    public PlayerInput playerInput;
    private Movement Movement;
    public Animator _anim;

    public bool IsGrounded;

    public float turnSmooth = 0.1f;
    float turnSmoothVelocity;

    public int jumpCount = 0;

    public float speed;
    public float JumpHeight;

    [SerializeField] private float animationLightAttackFinishTime = 0.5f;
    [SerializeField] private float animationHeavyAttackFinishTime = 0.5f;
    public float Height = 2.0f;
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
        AnimateRun(inputVector);
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
            Attack();
            isAttackingGoing = true;

            if (isAttacking && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= animationLightAttackFinishTime)
            {
                isAttacking = false;
            }
        }
    }

    public void HeavyAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            HeavyAttack();
            isHeavyAttackingGoing = true;

            if (isHeavyAttacking && _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= animationHeavyAttackFinishTime)
            {
                isHeavyAttacking = false;
            }
        }
    }

    void AnimateRun(Vector3 desiredDirection)
    {
        isRunning = (inputVector.x > 0 || inputVector.x < -0.0001f) || (inputVector.y > 0 || inputVector.y < -0.0001f) ? true : false;
        _anim.SetBool("IsRunning", isRunning);
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

    void Attack()
    {
        if (!isAttacking)
        {
            _anim.SetTrigger("Attacking?");
            isAttacking = true;
        }
    }

    void HeavyAttack()
    {
        if (!isHeavyAttacking)
        {
            _anim.SetTrigger("HeavyAttacking?");
            isHeavyAttacking = true;
        }
    }
}
