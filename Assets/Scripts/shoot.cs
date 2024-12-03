using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Shoot : MonoBehaviour
{

    public GameObject arow;
    public GameObject box;
    public float speed;

    private Rigidbody r;

    // Start is called before the first frame update
    void Start()
    {
        r = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        Destroy(gameObject);
        speed += 10;
        r.AddForce(Vector3.MoveTowards(box.transform.position, arow.transform.position, speed * -1));
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        speed += 10;
        r.AddForce(Vector3.MoveTowards(box.transform.position, arow.transform.position, speed * -1));

    }


    // Update is called once per frame
    void Update()
    {


        
        //dir = arow.transform.position - box.transform.position;
        //dir.Normalize();


    }
}
