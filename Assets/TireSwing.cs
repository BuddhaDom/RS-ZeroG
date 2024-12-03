using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class TireSwing : MonoBehaviour
{
    [Header("Rope Properties")]
    [SerializeField] private Transform ropeAnchor;
    private readonly List<RopeUnit> _ropeUnits = new();
    [SerializeField] private RopeUnit ropeUnitBase;
    [SerializeField] private Transform ropeContainer;
    [SerializeField] private RopeUnit tireRopeUnit;
    public int ropeLength;
    private int _lastRopeLength;

    private const int LengthCap = 100;
    
    private RopeUnit lastRopeUnit => _ropeUnits.Last();


    private void Start()
    {
        _ropeUnits.Add(ropeUnitBase);
        SetLength(ropeLength); 
        TieAndReposition(tireRopeUnit);
    }

    private void Update()
    {
        if (ropeLength != _lastRopeLength)
        {
            SetLength(ropeLength);
        }
        ropeUnitBase.transform.position = ropeAnchor.position;
    }

    private void SetLength(int length)
    {
        length = math.clamp(length, 1, LengthCap);
        if (length == _ropeUnits.Count) return;
        
        if (length > _ropeUnits.Count)
            for (int i = 0; i < length - _ropeUnits.Count; i++)
                AddRopeUnit();
        else if (length < _ropeUnits.Count)
            for (int i = 0; i < _ropeUnits.Count - length; i++)
                RemoveRopeUnit();
        
        TieAndReposition(tireRopeUnit);
    }

    private void AddRopeUnit()
    {
        var unit = Instantiate(lastRopeUnit, ropeContainer);
        // unit.transform.localPosition = 
        //     lastRope.transform.localPosition + 
        //     lastRope._joint.anchor + 
        //     Vector3.down;
        TieAndReposition(unit);
    }

    private void RemoveRopeUnit()
    {
        Destroy(lastRopeUnit.gameObject);
        _ropeUnits.RemoveAt(_ropeUnits.Count - 1);
    }

    private void TieAndReposition(RopeUnit unit)
    {
        unit.transform.position = lastRopeUnit.connectionPoint.position;
        unit.TieTo(lastRopeUnit);
        if(unit != tireRopeUnit) _ropeUnits.Add(unit);
    }
}
