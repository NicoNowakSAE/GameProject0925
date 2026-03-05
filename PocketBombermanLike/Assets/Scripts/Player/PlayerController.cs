using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(PlayerInput), typeof(RigidbodyMovement), typeof(GroundCheck))]
[RequireComponent(typeof(Health), typeof(Timer))]
public class PlayerController : MonoBehaviour
{
    private PlayerInput _playerInput;
    private RigidbodyMovement _rbMovement;
    private PlayerStatsSystem _statsSystem;
    private BombController _bombController;
    private PlayerAnimationController _animController;

    private GroundCheck _groundCheck;
    private float _groundedTimer;
    private Health _health;
    private int _extraJumpsRemaining;
    private int _lastReceivedJumpsRemainingValue;

    private bool _isJumpQueued = false;

    [Header("Movement Controls")]
    [SerializeField] private int _totalExtraJumpsAvailable = 1;
    [SerializeField] private bool _canJump = true;
    [SerializeField] private bool _canMove = true;
    [SerializeField][Range(0, 0.5f)] private float _coyoteTime = 0.13f;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();
        _rbMovement = GetComponent<RigidbodyMovement>();
        _groundCheck = GetComponent<GroundCheck>();
        _bombController = GetComponent<BombController>();
        _statsSystem = GetComponent<PlayerStatsSystem>();
        _health = GetComponent<Health>();
        _animController = GetComponent<PlayerAnimationController>();

        _statsSystem.Init();
        _rbMovement.Init(_statsSystem.GetStats);
        _bombController.Init(_statsSystem.GetStats, _statsSystem.GetMaxStats.BombCount);
        // _bomb.Init(reference)

        _extraJumpsRemaining = _totalExtraJumpsAvailable;
        _lastReceivedJumpsRemainingValue = _extraJumpsRemaining;
    }
    private void FixedUpdate()
    {
        if (_groundCheck.Check())
        {
            _extraJumpsRemaining = _totalExtraJumpsAvailable;
            _groundedTimer = _coyoteTime;
        }

        if (_rbMovement.Velocity.y > 0.1f)
            _groundedTimer = 0;

        if (_isJumpQueued)
        {
            _isJumpQueued = false;

            if (!_canJump)
                return;

            if (_groundedTimer <= 0) // if _groundedTimer > 0 then isGrounded
            {
                if (_extraJumpsRemaining > 0)
                {
                    _extraJumpsRemaining -= 1;
                }
                else
                {
                    return;
                }
            }

            _rbMovement.Jump();
        }

        if (_canMove)
            _rbMovement.Move(_playerInput.Move.ReadValue<Vector2>());
    }

    private void Update()
    {
        if (_playerInput.Jump.WasPressedThisFrame() && !_isJumpQueued)
            _isJumpQueued = true;
        
        if (_playerInput.TogglePause.WasPressedThisFrame())
            LevelController.Instance.ToggleState();

#if UNITY_EDITOR
        if (_lastReceivedJumpsRemainingValue != _totalExtraJumpsAvailable)
        {
            _lastReceivedJumpsRemainingValue = _totalExtraJumpsAvailable;
            _extraJumpsRemaining = _totalExtraJumpsAvailable;
        }
#endif

        _groundedTimer -= Time.deltaTime;


        _animController.AnimationStep(_playerInput);
    }

}