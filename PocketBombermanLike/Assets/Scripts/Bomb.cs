using UnityEngine;
using System;

public class Bomb : MonoBehaviour
{
    [SerializeField]
    private float _explodeTimer = 2;

    [SerializeField]
    private Collider2D _triggerCollider;
    [SerializeField]
    private Collider2D _collider;

    [SerializeField]
    private GameObject _explosionPrefab;

    private Action<Bomb> _despawnCallback;

    [SerializeField] private LayerMask _bombHitLayerMask;

    private int _bombRadius;
    private float _radius => _bombRadius * 2 + 1 - 0.1f;

    public void Init(Action<Bomb> callback)
    {
        _despawnCallback = callback;
    }

    public void Spawn(Vector3 pos, int radius)
    {
        float x = Mathf.Floor(pos.x) + 0.5f;
        float y = Mathf.Floor(pos.y) + 0.5f;
        transform.position = new Vector3(x, y, 0);

        _triggerCollider.enabled = true;
        _collider.enabled = false;

        _bombRadius = radius;

        Invoke("TriggerExplosion", _explodeTimer);
    }

    public void TriggerExplosion()
    {
        Instantiate(_explosionPrefab, this.transform.position, Quaternion.identity);

        Collider2D[] hits = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(_radius, 0.9f), 0, _bombHitLayerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            IBombHit h = hits[i].gameObject.GetComponent<IBombHit>();
            if (h == null) continue;
            h.Hit(1);
        }

        hits = Physics2D.OverlapBoxAll(this.transform.position, new Vector2(_radius, 0.9f), 90, _bombHitLayerMask);

        for (int i = 0; i < hits.Length; i++)
        {
            IBombHit h = hits[i].gameObject.GetComponent<IBombHit>();
            if (h == null) continue;
            h.Hit(1);
        }

        AudioManager.Instance.PlaySound("TestSound01");
        _despawnCallback.Invoke(this);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") == false)
            return;

        _triggerCollider.enabled = false;
        _collider.enabled = true;

    }

    public void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(this.transform.position, new Vector3(_radius, 0.9f, .5f));
        Gizmos.DrawWireCube(this.transform.position, new Vector3(0.9f, _radius, .5f));
    }
}


