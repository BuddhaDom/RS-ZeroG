using UnityEngine;

public class SelfDestroy : MonoBehaviour
{
    public float lifetime = 1f;
    
    private void Start() => Destroy (gameObject, lifetime);
}
