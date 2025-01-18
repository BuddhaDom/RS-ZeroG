using UnityEngine;
using UnityEngine.InputSystem;

public class PinchToZoom : MonoBehaviour
{
    public string targetTag = "Map";
    public float minScale = 0.1f;
    public float maxScale = 3f;
    public float maxSize = 2f; // Change this to cap maximum size, 2 felt good for a cube with size 1

    private float initialDistance;
    private Vector3 initialScale;
    private Transform target;

    void Update()
    {

        var touches = Touchscreen.current.touches;

        if (touches.Count >= 2 && touches[0].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved && touches[1].phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
        {
            if (target == null)
            {
                Ray ray = Camera.main.ScreenPointToRay(touches[0].position.ReadValue());
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag(targetTag))
                    {
                        target = hit.transform;
                    }
                }
            }

            if (target != null && target.CompareTag(targetTag))
            {
                var touch1 = touches[0];
                var touch2 = touches[1];

                Vector2 touch1Position = touch1.position.ReadValue();
                Vector2 touch2Position = touch2.position.ReadValue();

                float currentDistance = Vector2.Distance(touch1Position, touch2Position);

                if (initialDistance == 0f)
                {
                    initialDistance = currentDistance;
                    initialScale = target.localScale;
                }
                else
                {
                    float scaleFactor = currentDistance / initialDistance;
                    Vector3 newScale = initialScale * scaleFactor;

                    Renderer targetRenderer = target.GetComponent<Renderer>();
                    Collider targetCollider = target.GetComponent<Collider>();
                    float currentWorldSize = 0;

                    if (targetRenderer != null)
                    {
                        currentWorldSize = (newScale.magnitude * targetRenderer.bounds.size.magnitude);
                    }
                    else if (targetCollider != null)
                    {
                        currentWorldSize = (newScale.magnitude * targetCollider.bounds.size.magnitude);
                    }

                    if (currentWorldSize > maxSize)
                    {
                        float correctionScaleFactor = maxSize / currentWorldSize;
                        newScale *= correctionScaleFactor;
                    }

                    newScale = Vector3.Max(newScale, Vector3.one * minScale);
                    newScale = Vector3.Min(newScale, Vector3.one * maxScale);
                    target.localScale = newScale;
                }
            }
        }
        else if (initialDistance != 0f)
        {
            initialDistance = 0f;
            target = null;
        }
    }
}