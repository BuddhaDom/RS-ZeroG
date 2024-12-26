using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Collections.Generic;

public class mapMovement : MonoBehaviour
{
    public ARRaycastManager raycastManager; // Drag your ARRaycastManager here in the Inspector
    public Camera arCamera; // Drag your AR Camera here in the Inspector
    private GameObject selectedObject; // Currently selected object for manipulation
    private Vector2 startTouchPosition; // Start position of touch for drag detection
    private bool isDragging = false;

    void Update()
    {
        // Handle touch input
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                HandleTouchStart(touch.position);
            }
            else if (touch.phase == TouchPhase.Moved && selectedObject != null)
            {
                HandleDragging(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                isDragging = false;
                selectedObject = null;
            }
        }
    }

    private void HandleTouchStart(Vector2 touchPosition)
    {
        // Perform AR raycast to detect surfaces
        List<ARRaycastHit> hits = new List<ARRaycastHit>();
        if (raycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            // If a plane is hit, get the hit pose
            Pose hitPose = hits[0].pose;

            // Perform a physics raycast to check if a 3D object is touched
            Ray ray = arCamera.ScreenPointToRay(touchPosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                // Check if the touched object is interactable (e.g., has a specific tag)
                if (hit.transform.CompareTag("Interactable"))
                {
                    selectedObject = hit.transform.gameObject;
                    startTouchPosition = touchPosition;
                    isDragging = true;
                }
            }
        }
    }

    private void HandleDragging(Touch touch)
    {
        Vector2 currentTouchPosition = touch.position;
        Vector2 dragDelta = currentTouchPosition - startTouchPosition;

        // Expand/Shrink Object Based on Vertical Drag
        float scaleChange = dragDelta.y * 0.01f;
        selectedObject.transform.localScale += Vector3.one * scaleChange;

        // Spin Object Based on Horizontal Drag
        float spinAngle = dragDelta.x * 0.2f; // Adjust sensitivity as needed
        selectedObject.transform.Rotate(Vector3.up, spinAngle, Space.World);

        // Paint Object with Random Color
        Renderer objectRenderer = selectedObject.GetComponent<Renderer>();
        if (objectRenderer != null)
        {
            objectRenderer.material.color = new Color(Random.value, Random.value, Random.value);
        }

        // Update starting position for smooth dragging
        startTouchPosition = currentTouchPosition;
    }
}
