
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MovingPlatform : MonoBehaviour, IRideable
{
    private List<RiderData> _riderData = new List<RiderData>();
    private PathNavigation _pathNav;

    private void Awake()
    {
        _pathNav = GetComponent<PathNavigation>();
    }

    public void Attach(GameObject attachObject)
    {
        RiderData riderData = new RiderData()
        {
            Rigidbody = attachObject.GetComponent<Rigidbody2D>(),
            Parent = attachObject.transform.parent,
            GameObject = attachObject
        };

        Debug.Log($"[MOVING PLATFORM] Running Attach() for object {attachObject.name} with data: {riderData.ToString()} -");

        _riderData.Add(riderData);

        // attachObject.transform.SetParent(this.transform);

        // riderData.Rigidbody.bodyType = RigidbodyType2D.Kinematic;
    }

    public void Detach(GameObject detachObject)
    {
        RiderData riderData = _riderData.First(rData => rData.GameObject == detachObject);

        // if (riderData.IsNull)
        //     return;

        Debug.Log($"[MOVING PLATFORM] Running Detach() for object {detachObject.name} with data: {riderData.ToString()} -");

        // riderData.Rigidbody.bodyType = RigidbodyType2D.Dynamic;

        // detachObject.transform.SetParent(riderData.Parent);

        _riderData.RemoveAll(obj => obj.GameObject == detachObject);
    }

    private void FixedUpdate()
    {
        Vector2 lastFrameDelta = _pathNav.LastFrameDelta;

        foreach (RiderData data in _riderData)
        {
            if (data.Rigidbody.linearVelocity.sqrMagnitude < 5.0f)
            {
                Vector2 targetPos = data.Rigidbody.position + lastFrameDelta;
                data.Rigidbody.MovePosition(targetPos);
            }
                
        }
    }
}
