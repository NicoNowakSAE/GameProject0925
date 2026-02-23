using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(GroundCheck))]
public class ObjectRider : MonoBehaviour
{
    [SerializeField] private LayerMask _rideableLayers;
    private IRideable _currRideableObj = null;
    private GameObject _currRidableGameObject = null;

    private Rigidbody2D _rb;

    public bool IsRiding => _currRideableObj != null;
    private GroundCheck _groundCheck;

    private void Awake()
    {
        _groundCheck = GetComponent<GroundCheck>();
        _rb = GetComponent<Rigidbody2D>();
    }

    private bool IsObjectRideable(GameObject obj, out IRideable rideableObject)
    {
        IRideable rideable = obj.GetComponent<IRideable>();

        print($"[OBJECT RIDER] Is Rideable {obj.name}: {rideable!=null} -");

        if (rideable == null)
        {
            rideableObject = null;
            return false;
        }

        rideableObject = rideable;
        return true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        IRideable rideable;
        bool isGrounded = _groundCheck.Check(out Collider2D collisionObj);
        
        if (!isGrounded)
            return;

        if ((_rideableLayers.value & (1 << collisionObj.gameObject.layer)) == 0)
            return;

        if (IsObjectRideable(collision.gameObject, out rideable))
        {
            rideable.Attach(this.gameObject);
            _currRideableObj = rideable;
            _currRidableGameObject = collision.gameObject;

            print($"[OBJECT RIDER] Now riding: {collision.gameObject.name} -");
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (_currRideableObj == null)
            return;

        _currRideableObj.Detach(this.gameObject);
        print($"[OBJECT RIDER] Detached from: {_currRidableGameObject.name} -");
        _currRidableGameObject = null;
        _currRideableObj = null;
    }
}
