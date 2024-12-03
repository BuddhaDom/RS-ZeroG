using UnityEngine;

public class RopeUnit : MonoBehaviour
{
    [SerializeField] private ConfigurableJoint joint;
    [SerializeField] private Rigidbody rb;
    public Transform connectionPoint;

    public void TieTo(Rigidbody body) =>
        joint.connectedBody = body;
    
    public void TieTo(RopeUnit unit) =>
        TieTo(unit.rb);
}
