using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;
using Random = UnityEngine.Random;

public class TireSwing : MonoBehaviour
{
    [Header("Strength Values")] 
    public float strengthMagnitude;
    public Func<float> LerpFunction = null;
    
    [Header("Rope Properties")]
    private readonly List<RopeUnit> ropeUnits = new();
    [SerializeField] private RopeUnit ropeUnitBase;
    [SerializeField] private Transform ropeContainer;
    [SerializeField] private RopeUnit tireRopeUnit;
    private int ropeLength;
    private int lastRopeLength;
    [SerializeField] private LayerMask swingExcludeLayer;

    private const int LengthCap = 100;
    
    private RopeUnit lastRopeUnit => ropeUnits.Last();


    private void Start()
    {
        GameManager.RopeLengthUpdated.AddListener(SetLength);
        GameManager.RopeStrengthReleased.AddListener(RandomStrengthApplication);
        
        ropeUnits.Add(ropeUnitBase);
        SetLength(ropeLength); 
        TieAndReposition(tireRopeUnit);
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
        
        foreach (var ropeUnit in ropeUnits)
            ropeUnit.ropeCollider.excludeLayers = LayerMask.GetMask();
        lastRopeUnit.ropeCollider.excludeLayers = swingExcludeLayer;
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
        var force = new Vector3(Random.value*2-1, Random.value*2-1, Random.value*2-1).normalized;
        var position =
            tireRopeUnit.ropeCollider.transform.position +
            new Vector3(Random.value*2-1, Random.value*2-1, Random.value*2-1).normalized
            * 0.5f * tireRopeUnit.transform.lossyScale.x;
        
        tireRopeUnit.rb.AddForceAtPosition(force * strength * strengthMagnitude, position);
        
        Debug.DrawLine(Vector3.Lerp(position-force, position, 0.8f), position, Color.red, 2f);
    }
}
