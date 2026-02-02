using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    private GameInput _input;

    private InputAction _jump;
    public InputAction Jump => _jump;
    private InputAction _move;
    public InputAction Move => _move;

    private void Awake()
    {
        _input = new GameInput();

        _jump = _input.Player.Jump;
        _move = _input.Player.Move;
    }

    private void OnEnable()
    {
        _input.Enable();
    }

    private void OnDisable()
    {
        _input.Disable();
    }
}
