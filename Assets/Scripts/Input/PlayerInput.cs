using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using UnityEngineInternal;
using static UnityEngine.Rendering.DebugUI;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;

    [Header("Rotation")]
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;
    [SerializeField] private float speed;

    [Header("Gravity")]
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [Header("Jumping")]
    [SerializeField] private float jumpPower;
    [SerializeField] private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;

    [Header("Dash Variables")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 1000f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Header("Animator")]
    private Animator _anim;

    [Header("UI")]
    [SerializeField] private GameObject Panel;

    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();
    }

    private void Update()
    {
        ApplyGravity();
        ApplyRotation();
        ApplyMovement();
        _anim.SetBool("IsGrounded", IsGrounded());
    }

    private void ApplyRotation()
    {
        if (_input.sqrMagnitude == 0) return;

        var targetAngle = Mathf.Atan2(_direction.x, _direction.z) * Mathf.Rad2Deg;
        var angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _currentVelocity, smoothTime);
        transform.rotation = Quaternion.Euler(0.0f, angle, 0.0f);
    }

    private void ApplyMovement()
    {
        _characterController.Move(_direction * speed * Time.deltaTime);
        _anim.SetFloat("Velocity", _input.sqrMagnitude);
    }

    private void ApplyGravity()
    {
        if (IsGrounded() && _velocity < 0.0f)
        {
            _velocity = -1.0f;
        }
        else
        {
            _velocity += _gravity * gravityMultiplier * Time.deltaTime;
        }

        _direction.y = _velocity;
    }

    public void Move(InputAction.CallbackContext context)
    {
        _input = context.ReadValue<Vector2>();
        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.started) return;
        if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
        if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

        _numberOfJumps++;
        _velocity = jumpPower;
        _anim.SetInteger("JumpCount", _numberOfJumps);
    }

    private IEnumerator WaitForLanding()
    {
        yield return new WaitUntil(() => !IsGrounded());
        yield return new WaitUntil(IsGrounded);

        _numberOfJumps = 0;
        _anim.SetInteger("JumpCount", 0);

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

    public void Pause(InputAction.CallbackContext context)
    {
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
        _direction = new Vector3(_input.x, 0.0f, _input.y);
        _characterController.Move(_direction * dashingPower * Time.deltaTime);
        yield return new WaitForSeconds(dashingTime);
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
    private bool IsGrounded() => _characterController.isGrounded;
}
