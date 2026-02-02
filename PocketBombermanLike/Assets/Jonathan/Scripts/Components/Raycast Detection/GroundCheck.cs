using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Transform))]
public class GroundCheck : MonoBehaviour
{
    [SerializeField] private Vector2 _checkBoxDimensions;
    [SerializeField] private Vector2 _checkBoxOffset;
    [SerializeField] private LayerMask _groundLayers;
    public LayerMask SelectedLayers => _groundLayers;

    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        if (_transform != null)
        {
            Gizmos.DrawWireCube(
                _transform.position + (Vector3)(_checkBoxOffset * Vector3.one),
                _checkBoxDimensions
            );
        }
    }

    public bool Check()
    {
        Collider2D collider = Physics2D.OverlapBox(
            new Vector2(_transform.position.x, _transform.position.y) + _checkBoxOffset,
            _checkBoxDimensions,
            0.0f,
            _groundLayers
        );

        if (collider != null)
            return true;
        else
            return false;

    }
}
