using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(PinchToZoom), typeof(mapMovement))]
public class Map : MonoBehaviour
{
    public float RopeSwingScale = 1f;
    public TireSwing tireSwingPrefab;
    public SwingAnchor startingAnchor;

    private void Start()
    {
        if (GameManager.TireSwing != null) Destroy(GameManager.TireSwing);
        GameManager.DataSaver.last_ancor = startingAnchor;
        Instantiate(tireSwingPrefab, startingAnchor.transform)
            .transform.SetLocalPositionAndRotation(Vector3.zero,quaternion.identity);
    }
}
