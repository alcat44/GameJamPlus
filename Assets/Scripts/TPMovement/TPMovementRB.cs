using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class TPMovementRB : MonoBehaviour
{
    #region Component

    private Transform _cameraTransform;
    private Rigidbody _rigidbody;
    private Animator _animator;

    #endregion

    #region Setting

    [Header("Move")]
    [SerializeField] float _walkSpeed = 2.5f;
    [SerializeField] float _runSpeed = 5f;
    [SerializeField] float _sprintSpeed = 10f;
    [SerializeField] float _crouchSpeed = 1.25f;

    [Header("Facing Speed")]
    [SerializeField] float _normalFacing = 3f;
    [SerializeField] float _sprintFacing = 0.15f;

    [Header("Jump")]
    [SerializeField] float _jumpHeight = 2.5f;

    [Header("Gravity")]
    [SerializeField] float _gravity = -9.8f;
    [SerializeField] float _gravityScale = 3;

    #endregion

    #region Modifier

    private Vector2 _moveAxis;
    private Vector3 _direction;
    private Vector3 _directionForward;
    private Vector3 _velocity;
    private float _facingSpeed;
    private bool _isCrouching = false;
    private bool _isGrounded = true;

    public Vector3 Velocity { get => _velocity; }

    #endregion

    #region Invincibility Settings

    [Header("Invincibility Settings")]
    [SerializeField] private float invincibilityDuration = 5f; // Durasi kebal dalam detik
    [SerializeField] private Collider protectionZoneTrigger; // Trigger kebal yang bisa diatur di Inspector
    private bool isInvincible = false;
    private Coroutine invincibilityCoroutine;
    

    #endregion

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other == protectionZoneTrigger)
        {
            if (invincibilityCoroutine != null)
            {
                StopCoroutine(invincibilityCoroutine);
            }
            invincibilityCoroutine = StartCoroutine(InvincibilityTimer());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other == protectionZoneTrigger && invincibilityCoroutine != null)
        {
            StopCoroutine(invincibilityCoroutine);
            isInvincible = false;
            invincibilityCoroutine = null;
        }
    }

    private IEnumerator InvincibilityTimer()
    {
        isInvincible = true;
        yield return new WaitForSeconds(invincibilityDuration);
        isInvincible = false;
        invincibilityCoroutine = null;
    }

    public bool IsInvincible()
    {
        return isInvincible;
    }

    private void CalculateDirection()
    {
        Vector3 zDir = _cameraTransform.forward, xDir = _cameraTransform.right;
        zDir.y = 0; xDir.y = 0;
        _direction = (zDir * _moveAxis.y + xDir * _moveAxis.x).normalized;
        _directionForward = transform.forward.normalized;
    }

    private void ApplyFacing(float speed)
    {
        if (_direction.magnitude > 0)
        {
            transform.rotation = Quaternion.Lerp
            (
                transform.rotation,
                Quaternion.LookRotation(_direction),
                speed * 10f * Time.deltaTime
            );
        }
    }

    private float _speedMultiplier = 1f;

    public void SetSpeedMultiplier(float multiplier)
    {
        _speedMultiplier = multiplier;
    }

    private void SetVelocity(Vector3 direction, float speed)
    {
        _velocity.x = direction.x * speed * _speedMultiplier;
        _velocity.z = direction.z * speed * _speedMultiplier;
    }

    private void ApplyGravity()
    {
        float fallSpeedLimit = _gravity * _gravityScale;
        _velocity.y = _velocity.y > fallSpeedLimit ? _velocity.y + (_gravity * _gravityScale * Time.deltaTime) : fallSpeedLimit;
    }

    private void UpdateAnimations()
    {
        bool isWalking = _moveAxis.magnitude > 0 && !Input.GetKey(KeyCode.LeftShift) && !_isCrouching;
        bool isRunning = _moveAxis.magnitude > 0 && Input.GetKey(KeyCode.LeftShift) && !_isCrouching;
        bool isCrouching = _isCrouching;
        bool isCrouchWalking = _isCrouching && _moveAxis.magnitude > 0;
        bool isJumping = !_isGrounded;

        _animator.SetBool("isWalking", isWalking);
        _animator.SetBool("isRunning", isRunning);
        _animator.SetBool("isCrouching", isCrouching);
        _animator.SetBool("isJumping", isJumping);

        if (!isWalking && !isRunning && !isCrouchWalking)
        {
            _animator.SetBool("isIdle", true);
        }
        else
        {
            _animator.SetBool("isIdle", false);
        }
    }

    public void OnUpdate(Vector2 moveAxis)
    {
        _moveAxis = moveAxis;
        _isGrounded = Physics.Raycast(transform.position, Vector3.down, 1.1f);

        CalculateDirection();
        ApplyFacing(_facingSpeed);
        ApplyGravity();
        UpdateAnimations();

        if (Input.GetKey(KeyCode.LeftControl))
        {
            _isCrouching = true;

            if (_moveAxis.magnitude > 0)
            {
                CrouchWalk();
            }
            else
            {
                CrouchIdle();
            }
        }
        else if (Input.GetKey(KeyCode.LeftShift) && _moveAxis.magnitude > 0)
        {
            _isCrouching = false;
            Run();
        }
        else if (_moveAxis.magnitude > 0)
        {
            _isCrouching = false;
            Walk();
        }
        else
        {
            _isCrouching = false;
            Idle();
        }

        _rigidbody.velocity = _velocity;
    }

    public void Idle()
    {
        SetVelocity(Vector3.zero, 0f);
    }

    public void Walk()
    {
        _facingSpeed = _normalFacing;
        SetVelocity(_direction, _walkSpeed);
    }

    public void Run()
    {
        _facingSpeed = _normalFacing;
        SetVelocity(_direction, _runSpeed);
    }

    public void CrouchIdle()
    {
        SetVelocity(Vector3.zero, 0f);
    }

    public void CrouchWalk()
    {
        _facingSpeed = _normalFacing;
        SetVelocity(_direction, _crouchSpeed);
    }

    public void Jump()
    {
        if (_isGrounded && !_isCrouching)
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * (_gravity * _gravityScale));
            _animator.SetBool("isJumping", true);
        }
    }
}