using System;
using UnityEngine;

public class RopeUnit : MonoBehaviour
{
    public ConfigurableJoint joint;
    public Rigidbody rb;
    public Transform connectionPoint;
    
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
