using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering.Universal;

public class PlayerInput : MonoBehaviour
{
    public Rigidbody _rb;
    public PlayerInput playerInput;
    private Movement Movement;

    public bool IsGrounded;


    public int jumpCount = 0;

    public float speed;
    public float JumpHeight;

    public float Height = 2.0f;

    [Header("Dash Settings")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 24f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    // Start is called before the first frame update
    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        Movement = new Movement();

        Movement.Player.Enable();
    }

    void FixedUpdate()
    {
        if (isDashing)
        {
            return;
        }
        Vector2 inputVector = Movement.Player.Movement.ReadValue<Vector2>();
        _rb.AddForce(new Vector3(inputVector.x, 0, inputVector.y) * speed, ForceMode.Force);
        CheckIsGrounded();
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            jumpCount++;
            if (jumpCount < 2)
            {
                _rb.AddForce(Vector3.up * JumpHeight, ForceMode.Impulse);
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
            Debug.DrawRay(transform.position, Vector3.down * Height, UnityEngine.Color.green);
        }
        else
        {
            IsGrounded = false;
            Debug.DrawRay(transform.position, Vector3.down * Height, UnityEngine.Color.red);
        }
    }

}
