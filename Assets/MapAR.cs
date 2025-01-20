using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class MapAR : MonoBehaviour
{
    private ARRaycastManager rays;
    private ARAnchorManager anc;
    private ARPlaneManager plan;
    public MapHolder mapHolder;
    
    private ARAnchor oldAnchor;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rays = GetComponent<ARRaycastManager>();
        anc = GetComponent<ARAnchorManager>();
        plan = GetComponent<ARPlaneManager>();

        GameManager.MapAR = this;
    }

    public void AnchorMap()
    {
        var myHits = new List<ARRaycastHit>();
        ARPlane plane;
        ARAnchor point;
        ARRaycastHit nearest;

        if (Camera.main == null) return;
        var currentCamera = Camera.main.transform;

        var ray = new Ray(currentCamera.position, currentCamera.TransformDirection(Vector3.forward));

        var hit = rays.Raycast(ray, myHits, TrackableType.FeaturePoint | TrackableType.PlaneWithinPolygon);
        if (!hit) return;

        nearest = myHits.First();

        plane = plan.GetPlane(nearest.trackableId);
        
        if (plane != null) {
            point = anc.AttachAnchor(plane, nearest.pose); 
        } else {
            // Make sure the new GameObject has an ARAnchor component
            point = mapHolder.GetComponent<ARAnchor>();
            if (point == null)
            {
                point = mapHolder.gameObject.AddComponent<ARAnchor>();
            }
        }

        Destroy(oldAnchor);
        oldAnchor = point;
        mapHolder.transform.SetParent(point.transform, false);
        mapHolder.gameObject.SetActive(true);
    }
}
