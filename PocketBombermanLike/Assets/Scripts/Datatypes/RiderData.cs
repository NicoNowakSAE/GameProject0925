using System;
using UnityEngine;

[Serializable]
public struct RiderData
{
    public Rigidbody2D Rigidbody;
    public Transform Parent;
    public GameObject GameObject;

    public bool IsNull => (GameObject == null || Rigidbody == null || Parent == null);

    public override string ToString()
    {
        string goName = GameObject != null ? GameObject.name : "None";
        string parentName = Parent != null ? Parent.name : "None";
        string rbStatus = Rigidbody != null ? "Attached" : "Missing";

        return $"Rider: {goName} | Original Parent: {parentName} | Rigidbody: {rbStatus}";
    }
}