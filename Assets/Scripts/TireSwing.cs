using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Assertions;
using Random = UnityEngine.Random;

public class TireSwing : MonoBehaviour
{
    [Header("Strength Values")] 
    public float strengthMagnitude;
    public float cooldown;
    private float lastShotTime;
    
    [Header("Rope Properties")]
    [SerializeField] private Transform ropeAnchor;
    private readonly List<RopeUnit> ropeUnits = new();
    [SerializeField] private RopeUnit ropeUnitBase;
    [SerializeField] private Transform ropeContainer;
    [SerializeField] private RopeUnit tireRopeUnit;
    private int ropeLength;
    private int lastRopeLength;

    [Header("Raycast")] 
    [SerializeField] private LayerMask tireLayer;
    
    private const int LengthCap = 100;
    
    private RopeUnit lastRopeUnit => ropeUnits.Last();


    private void Start()
    {
        ropeUnits.Add(ropeUnitBase);
        SetLength(ropeLength); 
        TieAndReposition(tireRopeUnit);
        
        GameManager.RopeLengthUpdated.AddListener(SetLength);
        GameManager.RopeStrengthReleased.AddListener(ApplyStrengthAtScreenCenter);
        GameManager.TireSwing = this;
    }

    private void Update()
    {
        // if (ropeLength != lastRopeLength)
        // {
        //     SetLength(ropeLength);
        // }
        // ropeUnitBase.transform.position = ropeAnchor.position;
        
        lastRopeLength = ropeLength;
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
            currentCamera.TransformDirection(Vector3.forward).normalized * strength * strengthMagnitude, hit.point);
        Debug.DrawLine(hit.point, currentCamera.position , Color.red, 2);
        
        lastShotTime = Time.time;
    }
}
