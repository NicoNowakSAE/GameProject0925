using System;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody2D), typeof(Transform))]
public class RigidbodyMovement : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce = 10.0f;

    private Rigidbody2D _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 dir)
    {
        _rigidbody.linearVelocity = new Vector2(dir.x * _speed, _rigidbody.linearVelocity.y);
    }

    public void Jump()
    {
        float verticalForceRemaining = Mathf.Max(0, _jumpForce - _rigidbody.linearVelocity.y); // Sorgt dafür, dass man nicht höher springt, wenn man schnell hintereinander SPACE drückt :)

        _rigidbody.AddForce(
            verticalForceRemaining * Vector2.up,
            ForceMode2D.Impulse
        );
    }

}
