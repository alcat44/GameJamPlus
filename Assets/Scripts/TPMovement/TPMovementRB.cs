using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(Animator))]
public class TPMovementRB : MonoBehaviour
{
    #region Component

    private Transform _cameraTransform;     // Kamera yang menargetkan player
    private Rigidbody _rigidbody;           // Komponen Rigidbody yang terpasang pada player
    private Animator _animator;             // Komponen Animator yang terpasang pada player

    #endregion

    #region Setting

    [Header("Move")]
    [SerializeField] float _walkSpeed = 2.5f;           // Kecepatan berjalan
    [SerializeField] float _runSpeed = 5f;              // Kecepatan berlari
    [SerializeField] float _sprintSpeed = 10f;          // Kecepatan sprint
    [SerializeField] float _crouchSpeed = 1.25f;        // Kecepatan berjalan jongkok

    [Header("Facing Speed")]
    [SerializeField] float _normalFacing = 3f;          // Kecepatan rotasi normal
    [SerializeField] float _sprintFacing = 0.15f;       // Kecepatan rotasi saat sprint

    [Header("Jump")]
    [SerializeField] float _jumpHeight = 2.5f;          // Tinggi lompatan

    [Header("Gravity")]
    [SerializeField] float _gravity = -9.8f;            // Nilai gravitasi
    [SerializeField] float _gravityScale = 3;           // Skala gravitasi

    #endregion

    #region Modifier

    private Vector2 _moveAxis;          // Arah dari input
    private Vector3 _direction;         // Arah relatif
    private Vector3 _directionForward;  // Arah ke depan
    private Vector3 _velocity;          // Gerakan
    private float _facingSpeed;         // Kecepatan rotasi
    private bool _isCrouching = false;  // Status jongkok
    private bool _isGrounded = true;    // Status apakah player sedang di tanah

    public Vector3 Velocity { get => _velocity; }

    #endregion

    #region Unity Methods

    private void Start()
    {
        _cameraTransform = Camera.main.transform;
        _rigidbody = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
    }

    #endregion

    #region Private Methods

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

        // Check if player is grounded
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
        if (_isGrounded && !_isCrouching) // Lompat hanya jika di tanah dan tidak sedang jongkok
        {
            _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * (_gravity * _gravityScale));
            _animator.SetBool("isJumping", true);
        }
    }

    #endregion
}
