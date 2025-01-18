using UnityEngine;
using UnityEngine.UI;

public class Crosshair : MonoBehaviour
{
    private Transform currentCamera;
    private Image image;
    private Animator animator;
    private static readonly int Focused = Animator.StringToHash("Focused");
    
    [SerializeField] private LayerMask tireLayer;

    [SerializeField] private Color focusedColor;
    [SerializeField] private Color unfocusedColor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Get the active camera's transform
        currentCamera = (Camera.main ?? Camera.allCameras[0]).transform;
        image = GetComponent<Image>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        // Swap the color of the image whether it's hitting the swing or not.
        bool hit = Physics.Raycast(
            currentCamera.position,
            currentCamera.TransformDirection(Vector3.forward),
            Mathf.Infinity,
            tireLayer);

        image.color = hit ? focusedColor : unfocusedColor;
        animator.SetBool(Focused, hit);
    }
}
