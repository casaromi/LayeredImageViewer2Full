using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tablet : MonoBehaviour
{
    // Start is called before the first frame update
    Vector3 initialPos;
    Quaternion initialRot;


    void Start()
    {
        initialPos = transform.position;
        initialRot = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag=="ground")
        {
            transform.position = initialPos;
            transform.rotation = initialRot;
        }
    }
}
