using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.XR.ARFoundation;
using Random = UnityEngine.Random;

public class TireSwing : MonoBehaviour
{
    
    [Header("Physics")] 
    public float strengthMagnitude;
    public float cooldown;
    private float lastShotTime;
    [SerializeField] private float connectedMassStrengthMultiplier = 1f;
    
    [Header("Rope Properties")]
    [SerializeField] private Transform ropeAnchor;
    private readonly List<RopeUnit> ropeUnits = new();
    [SerializeField] private RopeUnit ropeUnitBase;
    [SerializeField] private Transform ropeContainer;
    [SerializeField] private RopeUnit tireRopeUnit;
    private int ropeLength;
    private int lastRopeLength;
    private float startingMass;
    private float startingAngularDamp;
    private float startingLinearDamp;
    private Vector3 worldPosition;
    private int physicsSkipFrames;
    private ARAnchor holdingAnchor;
    private Vector3 lastRelativePosition;

    private Vector3 lastLossyScale;

    [Header("Raycast")] 
    [SerializeField] private LayerMask tireLayer;
    
    private const int LengthCap = 100;
    
    private RopeUnit lastRopeUnit => ropeUnits.Last();


    private void Start()
    {
        holdingAnchor = GetComponentInParent<ARAnchor>();
        
        ropeUnits.Add(ropeUnitBase);
        SetLength(ropeLength); 
        TieAndReposition(tireRopeUnit);
        
        GameManager.RopeLengthUpdated.AddListener(SetLength);
        GameManager.RopeStrengthReleased.AddListener(ApplyStrengthAtScreenCenter);
        GameManager.TireSwing = this;
        
        lastLossyScale = Vector3.zero;
        
        startingMass = tireRopeUnit.rb.mass;
        startingAngularDamp = tireRopeUnit.rb.angularDamping;
        startingLinearDamp = tireRopeUnit.rb.linearDamping;
        worldPosition = transform.position;

        UpdatePhysicsVariables();
    }

    private void Update()
    {
        if (lastLossyScale != transform.lossyScale || ropeLength != lastRopeLength)
        {
            UpdatePhysicsVariables();
        }

        lastRelativePosition = holdingAnchor.transform.InverseTransformPoint(transform.position);
        lastRopeLength = ropeLength;
        lastLossyScale = transform.lossyScale;
    }

    private void UpdatePhysicsVariables()
    {
        var modifier = transform.lossyScale.magnitude / Mathf.Sqrt(3);
        foreach (var unit in ropeUnits)
        {
            //unit.rb.angularDamping = startingAngularDamp * modifier;
            //unit.rb.linearDamping = startingLinearDamp * modifier;
            unit.rb.mass = startingMass * modifier;
            unit.joint.connectedMassScale = 1f;
        }
        //tireRopeUnit.rb.angularDamping = startingAngularDamp * modifier;
        //tireRopeUnit.rb.linearDamping = startingLinearDamp * modifier;
        tireRopeUnit.rb.mass = startingMass * modifier;
        lastRopeUnit.joint.connectedMassScale = connectedMassStrengthMultiplier;
    }

    private void SetLength(int length)
    {
        length = math.clamp(length, 1, LengthCap);
        ropeLength = length;
        if (length == ropeUnits.Count) return;
        
        if (length > ropeUnits.Count)
            for (int i = 0; i < length - ropeUnits.Count; i++)
                AddRopeUnit();
        else if (length < ropeUnits.Count)
            for (int i = 0; i < ropeUnits.Count - length; i++)
                RemoveRopeUnit();
        
        TieAndReposition(tireRopeUnit);
        UpdatePhysicsVariables();
    }

    private void AddRopeUnit()
    {
        var unit = Instantiate(lastRopeUnit, ropeContainer);
        TieAndReposition(unit);
    }

    private void RemoveRopeUnit()
    {
        Destroy(lastRopeUnit.gameObject);
        ropeUnits.RemoveAt(ropeUnits.Count - 1);
    }

    private void TieAndReposition(RopeUnit unit)
    {
        unit.transform.position = lastRopeUnit.connectionPoint.position;
        unit.TieTo(lastRopeUnit);
        if(unit != tireRopeUnit) ropeUnits.Add(unit);
    }

    private void RandomStrengthApplication(float strength)
    {
        var force = new Vector3(Random.value, Random.value, Random.value).normalized;
        var position = 
            tireRopeUnit.transform.position + 
            new Vector3(Random.value, Random.value, Random.value).normalized 
            * 0.5f * tireRopeUnit.transform.lossyScale.x;
        
        tireRopeUnit.rb.AddForceAtPosition(force * strength * strengthMagnitude, position);
        
        Debug.DrawLine(position - force, position, Color.red, 2);
    }

    private void ApplyStrengthAtScreenCenter(float strength)
    {
        if (Time.time - lastShotTime < cooldown) return;
        
        Assert.IsNotNull(Camera.main, "No main camera found.");
        
        var currentCamera = Camera.main.transform;

        if (!Physics.Raycast(
                currentCamera.position, 
                currentCamera.TransformDirection(Vector3.forward), 
                out var hit, Mathf.Infinity,
                tireLayer)
           ) return;
        
        tireRopeUnit.rb.AddForceAtPosition(
            // This is the strength calculation
            currentCamera.TransformDirection(Vector3.forward).normalized * // The direction
            strength * strengthMagnitude * // The strength
            transform.lossyScale.magnitude / Mathf.Sqrt(3), // And adjusting strength for current size of the object
            hit.point);
        Debug.DrawLine(hit.point, currentCamera.position , Color.red, 2);
        
        lastShotTime = Time.time;
    }
}
