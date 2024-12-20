using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class move_anchor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public GameObject anchor;
    public GameObject place;

    private void OnCollisionEnter(Collision col)
    {
        if (col.gameObject == place)
        {   
            anchor.transform.position = place.transform.position + new Vector3 (0, 0, 0);

        }
    }
}
