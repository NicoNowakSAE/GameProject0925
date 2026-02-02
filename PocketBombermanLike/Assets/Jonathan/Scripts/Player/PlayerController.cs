using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(RigidbodyMovement), typeof(GroundCheck))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private RigidbodyMovement _rbMovement;
    private GroundCheck _groundCheck;
    private int _jumpsRemaining;
    private int _lastReceivedJumpsRemainingValue;

    private bool _isJumpQueued = false;

    [SerializeField] private int _totalJumpsAvailable = 1;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canMove = true;


    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rbMovement = GetComponent<RigidbodyMovement>();
        _groundCheck = GetComponent<GroundCheck>();

        _jumpsRemaining = _totalJumpsAvailable;
        _lastReceivedJumpsRemainingValue = _jumpsRemaining;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        bool isGrounded = _groundCheck.Check();

        if (isGrounded)
            _jumpsRemaining = _totalJumpsAvailable;
    }

    private void FixedUpdate()
    {
        if (_isJumpQueued)
        {
            _isJumpQueued = false;

            if (!_canJump)
                return;

            bool isGrounded = _groundCheck.Check();
            bool hasRemainingJumps = _jumpsRemaining > 0;

            if (!isGrounded && !hasRemainingJumps)
                return;

            _rbMovement.Jump();
            _jumpsRemaining -= 1;
        }
    }

    private void Update()
    {
        if (_playerInput.Jump.WasPressedThisFrame() && !_isJumpQueued)
            _isJumpQueued = true;

        if (_canMove)
            _rbMovement.Move(_playerInput.Move.ReadValue<Vector2>());

        if (_lastReceivedJumpsRemainingValue != _totalJumpsAvailable)
        {
            _lastReceivedJumpsRemainingValue = _totalJumpsAvailable;
            _jumpsRemaining = _totalJumpsAvailable;
        }
    }
}