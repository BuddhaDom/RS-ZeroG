using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class mapMovement : MonoBehaviour
{
    private Camera mainCamera;
    public string objectTag = "Map"; // Set tag name on object
    public float spinMultiplier = 5f;
    public float momentumDamping = 0.98f;
    public float minimumMomentum = 0.1f;

    private Vector3 currentRotationAxis;
    private float currentRotationSpeed;
    private Vector2 startTouchPosition;

    private void Start()
    {
        mainCamera = Camera.main;
    }

    void Update()
    {
        if (Touchscreen.current != null)
        {
            var touch = Touchscreen.current.primaryTouch;

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Began)
            {
                startTouchPosition = touch.position.ReadValue();
                currentRotationSpeed = 0;
            }

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Moved)
            {
                Vector2 currentTouchPosition = touch.position.ReadValue();
                Vector2 swipeDirection = currentTouchPosition - startTouchPosition;

                Ray ray = mainCamera.ScreenPointToRay(startTouchPosition);
                if (Physics.Raycast(ray, out RaycastHit hit))
                {
                    if (hit.transform.CompareTag(objectTag))
                    {
                        currentRotationAxis = new Vector3(swipeDirection.y, -swipeDirection.x, 0).normalized;
                        currentRotationSpeed = swipeDirection.magnitude * spinMultiplier;
                        hit.transform.Rotate(currentRotationAxis, currentRotationSpeed * Time.deltaTime, Space.World);
                        startTouchPosition = currentTouchPosition;
                    }
                }
            }

            if (touch.phase.ReadValue() == UnityEngine.InputSystem.TouchPhase.Ended)
            {
                currentRotationSpeed *= 0.99f; // Change here to reduce drag.
            }
        }

        if (currentRotationSpeed > minimumMomentum)
        {
            transform.Rotate(currentRotationAxis, currentRotationSpeed * Time.deltaTime, Space.World);
            currentRotationSpeed *= momentumDamping;
        }
    }
}