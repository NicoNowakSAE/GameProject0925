using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class DamageDealer : MonoBehaviour
{
    [SerializeField] private LayerMask _damageableLayers;
    [SerializeField] private float _damageToDeal;
    [SerializeField] private Vector2 _damageBoxDimensions;
    [SerializeField] private Vector2 _damageBoxOffset;

    private bool _isAnyEnemyInDamageBoxArea = false;
    private Transform _transform;

    private void Awake()
    {
        _transform = GetComponent<Transform>();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = _isAnyEnemyInDamageBoxArea ? Color.red : Color.green;

        Gizmos.DrawWireCube(
            transform.position + (Vector3)_damageBoxOffset,
            (Vector3)_damageBoxDimensions
        );
    }

    private GameObject[] GetObjectsInDamageArea()
    {
        Collider2D[] collider = Physics2D.OverlapBoxAll(
            _transform.position + (Vector3)_damageBoxOffset,
            _damageBoxDimensions,
            0f,
            _damageableLayers
        );

        return collider.Select(
            colObj => colObj.gameObject
            ).ToArray();
    }

    private void Update()
    {
        GameObject[] objectsInDamageArea = GetObjectsInDamageArea();

        if (objectsInDamageArea.Length == 0)
        {
            _isAnyEnemyInDamageBoxArea = false;
            return;
        }

        _isAnyEnemyInDamageBoxArea = true;

        foreach (GameObject gObj in objectsInDamageArea)
        {
            if (HealthCollection.GameObjectHealthPairs.ContainsKey(gObj))
            {
                Health health = gObj.GetComponent<Health>();
                health.Reduce(_damageToDeal);
            }
        }
    }
}
