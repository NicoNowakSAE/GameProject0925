using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Transform))]
public class RigidbodyMovement : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    public Vector2 Velocity { get => _rigidbody.linearVelocity; }

    PlayerStatsSystem.Stats _stats;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Init(PlayerStatsSystem.Stats stats)
    {
        _stats = stats;
    }

    public void Move(Vector2 dir)
    {
        _rigidbody.linearVelocity = new Vector2(dir.x * _stats.Speed, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        float verticalForceRemaining = Mathf.Max(0, _stats.JumpForce - _rigidbody.linearVelocity.y); // Sorgt dafür, dass man nicht höher springt, wenn man schnell hintereinander SPACE drückt :)

        _rigidbody.AddForce(
            verticalForceRemaining * Vector2.up,
            ForceMode2D.Impulse
        );
    }

}
