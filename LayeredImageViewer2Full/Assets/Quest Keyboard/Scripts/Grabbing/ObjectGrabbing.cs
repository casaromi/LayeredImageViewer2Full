using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectGrabbing : MonoBehaviour
{
    // Start is called before the first frame update

    //this is to know which hand has picked an object

    [Header("Grabbing this object")]
    //public bool isGrabbable=true;

    //basic gamecomponents
    //private Collider _col;
    private Rigidbody _rb;

    public Transform offsetR, offsetL;

    Vector3 deltaOffsetR, deltaOffsetL;

    public HandGrabbing handGrabScp;
    
    public int maskDefault;


    [Header("Releasing this object")]
    public float releaseSpeedFactor=1.5f;



    void Start()
    {
        _rb = GetComponent<Rigidbody>();
        //_col = GetComponent<Collider>();

        maskDefault =gameObject.layer;
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void LateUpdate()
    {

       
    }


    private void OnTriggerStay(Collider other)
    {
        //Debug.Log(other.name);
        if (other.CompareTag("handRight"))
        {
            other.gameObject.GetComponent<HandGrabbing>().potentialOnjectInHand = gameObject;
        }
        else if (other.CompareTag("handLeft"))
        {
            
             other.gameObject.GetComponent<HandGrabbing>().potentialOnjectInHand = gameObject;
        }

    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("handRight"))
        {
            other.gameObject.GetComponent<HandGrabbing>().potentialOnjectInHand = null;
        }
        else if (other.CompareTag("handLeft") )
        {
            other.gameObject.GetComponent<HandGrabbing>().potentialOnjectInHand = null;
        }


    }

    public void Release()
    {

        gameObject.transform.SetParent(null);

        _rb.isKinematic = false;
        _rb.useGravity = true;
        //_col.enabled = true;
        gameObject.layer = maskDefault;

        _rb.velocity =releaseSpeedFactor*handGrabScp.filteredSpeed;
        _rb.angularVelocity = releaseSpeedFactor * handGrabScp.filteredAngularSpeed;
    }

    public void RigidBodyToKn()
    {
        _rb.useGravity = false;
        _rb.isKinematic = true;
    }
}
