using UnityEngine;

/// <summary>
/// A navigation point component required for <see cref="PathNavigation"/>. 
/// </summary>
public class PathNavPoint : MonoBehaviour
{
    private Transform _transform;
    public Vector3 Position {
        get
        {
            if (_transform == null) _transform = transform;
            return _transform.position;
        }
    }

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, Vector3.one);
    }
}
