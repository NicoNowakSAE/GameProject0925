using Mono.Cecil;
using UnityEditor.VersionControl;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Performs a 2D line-of-sight check using a raycast
/// to detect targets in a given direction.
/// </summary>
public class LineOfSight : MonoBehaviour
{
    /// <summary>
    /// Maximum distance of the line-of-sight raycast.
    /// </summary>
    [Header("Line Of Sight Properties")]
    [SerializeField] private float _lineOfSightLength = 2.0f;

    /// <summary>
    /// Public read-only access to the line-of-sight length.
    /// </summary>
    public float LineOfSightLength => _lineOfSightLength;

    /// <summary>
    /// Layer mask defining which layers can be detected
    /// by the line-of-sight raycast.
    /// </summary>
    [SerializeField] private LayerMask _lineOfSightTargetLayers;

    /// <summary>
    /// Cached transform reference.
    /// </summary>
    private Transform _transform;

    /// <summary>
    /// Caches required component references.
    /// </summary>
    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    /// <summary>
    /// Checks if a target is hit in the given direction
    /// starting from the provided origin.
    /// </summary>
    /// <param name="direction">Normalized direction of the raycast.</param>
    /// <param name="origin">World position where the raycast starts.</param>
    /// <returns>
    /// True if a collider on the target layers is hit,
    /// otherwise false.
    /// </returns>
    public bool CheckTargets(Vector2 direction, Vector3 origin)
    {
        RaycastHit2D hit = Physics2D.Raycast(
            origin,
            direction,
            _lineOfSightLength,
            _lineOfSightTargetLayers
        );

        return hit.collider != null;
    }
}
