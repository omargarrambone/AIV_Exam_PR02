using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerInput : MonoBehaviour
{

    [Header("Movement")]
    [SerializeField] private float speed;
    [SerializeField] private float deadZone;
    private Vector2 _input;
    private CharacterController _characterController;
    private Vector3 _direction;
    public bool ShouldNotMove;

    [Header("Rotation")]
    [SerializeField] private float smoothTime = 0.05f;
    private float _currentVelocity;

    [Header("Gravity")]
    private float _gravity = -9.81f;
    [SerializeField] private float gravityMultiplier = 3.0f;
    private float _velocity;

    [Header("Jumping")]
    [SerializeField] private float jumpPower;
    [SerializeField] private int _numberOfJumps;
    [SerializeField] private int maxNumberOfJumps = 2;
    //[SerializeField] private bool _isRodPickedUp = false;

    [Header("Dash Variables")]
    private bool canDash = true;
    private bool isDashing;
    private float dashingPower = 3f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 0.6f;
    [SerializeField] private TrailRenderer _trail;


    [Header("Sounds")]
    public AudioSource Dash_SFX;
    public List<AudioSource> Jump_SFX;

    [Header("Animator and RigidBody")]
    private Animator _anim;

    [Header("UI")]
    [SerializeField] private GameObject Panel;

    [Header("Weapons")]
    [SerializeField] private WeaponsManager _weaponsManager;

    [Header("Footsteps")]
    public FootSteps _footSteps;


    private void Awake()
    {
        _characterController = GetComponent<CharacterController>();
        _anim = GetComponent<Animator>();

    }

    private void Update()
    {
        ApplyGravity();
        CheckIsGrounded();
        ApplyMovement();
        ApplyRotation();
    }

    public void LightAttack(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            if (ShouldNotMove) return;

            _anim.SetTrigger("IsAttacking");
            _weaponsManager.OnAttack(context);
        }

    }

    public void Kick(InputAction.CallbackContext context)
    {
        if (context.performed && IsGrounded())
        {
            if (ShouldNotMove) return;

            _anim.SetTrigger("IsKicking");

            PlayerManager.DisablePlayerMovement();
        }
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

    private void CheckIsGrounded()
    {

        _anim.SetBool("IsGrounded", IsGrounded());
    }

    public void Move(InputAction.CallbackContext context)
    {
        if (ShouldNotMove) { _input = Vector2.zero; _direction = Vector2.zero; return; }

        _input = context.ReadValue<Vector2>();

        if (Mathf.Abs(_input.x) > deadZone) _input.x = _input.x > 0 ? 1 : -1;
        if (Mathf.Abs(_input.y) > deadZone) _input.y = _input.y > 0 ? 1 : -1;

        _direction = new Vector3(_input.x, 0.0f, _input.y);
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (!context.performed) return;

        if (_weaponsManager.TakenWeapons[(int)ItemType.LongKatana] == false)
        {
            if (!IsGrounded()) return;
            if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

            _numberOfJumps++;
            _velocity = jumpPower;
            _anim.SetInteger("JumpCount", _numberOfJumps);
        }
        else
        {
            if (!IsGrounded() && _numberOfJumps >= maxNumberOfJumps) return;
            if (_numberOfJumps == 0) StartCoroutine(WaitForLanding());

            _numberOfJumps++;
            _velocity = jumpPower;
            _anim.SetInteger("JumpCount", _numberOfJumps);
        }

        if (_numberOfJumps == 1)
        {
            Jump_SFX[0].Play();
        }
        else
        {
            Jump_SFX[1].Play();
        }
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
            if (ShouldNotMove) return;

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
                InGameMenusManager.ShowHidePauseMenu(true);
            }
            else
            {
                InGameMenusManager.ShowHidePauseMenu(false);
            }
        }
    }

    public void Dash(InputAction.CallbackContext context)
    {
        if (context.performed && canDash)
        {
            if (GameManager.GameState == GameState.Paused) return;
            StartCoroutine(Dash());
            Dash_SFX.Play();
        }
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;

        _characterController.Move(transform.forward * dashingPower);
        _trail.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        _trail.emitting = false;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;

    }
    private bool IsGrounded() => _characterController.isGrounded;

    public void OnFootStep(AnimationEvent animationEvent)
    {
        if (animationEvent.animatorClipInfo.weight > 0.5f)
        {
            _footSteps.PlayFootstep();
        }
    }

}
