using System;
using UnityEngine;
using UnityEngine.Serialization;

public class RopeUnit : MonoBehaviour
{
    public ConfigurableJoint joint { get; private set; }
    public Rigidbody rb { get; private set; }
    public Transform connectionPoint;
    public Collider ropeCollider;
    
    private void Awake()
    {
        joint = GetComponent<ConfigurableJoint>();
        rb = GetComponent<Rigidbody>();
    }

    public void TieTo(Rigidbody body) =>
        joint.connectedBody = body;
    
    public void TieTo(RopeUnit unit) =>
        TieTo(unit.rb);
}
