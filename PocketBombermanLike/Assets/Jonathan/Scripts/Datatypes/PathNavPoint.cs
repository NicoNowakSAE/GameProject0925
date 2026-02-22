using TMPro;
using UnityEngine;

public class PathNavPoint : MonoBehaviour
{
    private Transform _transform;
    public Vector3 Position => _transform.position;

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
