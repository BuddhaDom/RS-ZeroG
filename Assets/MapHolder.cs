using System;
using UnityEngine;
using UnityEngine.Serialization;

public class MapHolder : MonoBehaviour
{
    public Transform mapPosition;
    public Map heldMap;
    public Map startingMap;

    private void Start()
    {
        heldMap = Instantiate(startingMap, mapPosition);
    }

    public void ChangeMap(Map map)
    {
        Destroy(heldMap.gameObject);
        heldMap = Instantiate(map, mapPosition);
    }
}
