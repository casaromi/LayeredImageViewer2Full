using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandGrabbing : MonoBehaviour
{
    //public OVRInput.Controller controller;

    // Start is called before the first frame update
    [Header("Grabbing")]
    public GameObject objectInHand=null;
    public GameObject potentialOnjectInHand=null;
    //public float filterValue = 0.05f;
    //public Transform parentHand;
    //public Transform controllerAnchor;
    
    public Vector3 filteredSpeed;
    public float filterValue = 0.1f;
    private Vector3 speed, pos_i, pos_i_1;

    public Vector3 filteredAngularSpeed;
    private Quaternion filteredQuat;
    private Quaternion angularSpeed, rot_i,rot_i_1;

    [Header("Animator of the hand")]
    public Animator animT;

    [Header("Colliders of fingers reference of armature")]
    public Transform armatureHand;
    public Collider[] colliders;

    [Header("Other hand")]
    public HandGrabbing otherHand;

    void Start()
    {
        colliders = armatureHand.GetComponentsInChildren<Collider>();
        EnableColliders(false);

        rot_i = transform.rotation;
        rot_i_1 = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
      
        if(potentialOnjectInHand!=null)
        {
            ObjectGrabbing objScp = potentialOnjectInHand.GetComponent<ObjectGrabbing>();

            //if ( OVRInput.GetDown(grabButton) && objScp.isGrabbable)
            bool grabCondition = (InputManager.instance.G_L_DW && gameObject.tag=="handLeft" 
                || (InputManager.instance.G_R_DW && gameObject.tag == "handRight"))                
                  && otherHand.objectInHand != potentialOnjectInHand;

            if (grabCondition)
            {

                objectInHand = potentialOnjectInHand;


                objScp.RigidBodyToKn();

                //_col.enabled = false;
                objectInHand.layer = (gameObject.layer);

                SetParent();
                objScp.handGrabScp = this;

                //DebugCanvas.DC.Log(objScp.gameObject.name + "-- pick--> " +gameObject.name);

                
            }

        }


        if(objectInHand!=null)
        {

            ObjectGrabbing objHandScp = objectInHand.GetComponent<ObjectGrabbing>();

            objectInHand.transform.position=transform.position;
            objectInHand.transform.rotation = transform.rotation;

            //offset
            if (gameObject.CompareTag("handLeft"))
            {
                if(objHandScp.offsetL!=null)
                {
                    objectInHand.transform.localPosition = new Vector3(-objHandScp.offsetL.localPosition.x * objHandScp.offsetL.parent.localScale.x,
                                                                       -objHandScp.offsetL.localPosition.y * objHandScp.offsetL.parent.localScale.y,
                                                                       -objHandScp.offsetL.localPosition.z * objHandScp.offsetL.parent.localScale.z
                                                            );
                    objectInHand.transform.localRotation = objHandScp.offsetL.localRotation;


                }
                
            }
            else if (gameObject.CompareTag("handRight"))
            {
                if (objHandScp.offsetR != null)
                {
                    objectInHand.transform.localPosition =new Vector3( -objHandScp.offsetR.localPosition.x* objHandScp.offsetR.parent.localScale.x,
                                                                       -objHandScp.offsetR.localPosition.y * objHandScp.offsetR.parent.localScale.y,
                                                                       -objHandScp.offsetR.localPosition.z * objHandScp.offsetR.parent.localScale.z
                                                            );
                    objectInHand.transform.localRotation = objHandScp.offsetR.localRotation;
                }
            }


            //release actions
            if ((InputManager.instance.G_L_UP && gameObject.tag == "handLeft"
                || (InputManager.instance.G_R_UP && gameObject.tag == "handRight")))
            {

                objHandScp.Release();

                //DebugCanvas.DC.Log(objHandScp.gameObject.name + "-- rlse --> " + gameObject.name);

                objectInHand = null;
            }

        }
          


    }

    public void FixedUpdate()
    {
        //obtain the speed of the hand
        pos_i = transform.position;

        speed = ( pos_i- pos_i_1) / Time.fixedDeltaTime;

        filteredSpeed = Vector3.Lerp(filteredSpeed, speed, filterValue);

        pos_i_1 = pos_i;

        //obtain the angular speed
        rot_i = transform.rotation;
                
        filteredQuat =Quaternion.Lerp( filteredQuat, rot_i * Quaternion.Inverse(rot_i_1), filterValue);

        filteredAngularSpeed = filteredQuat.eulerAngles;

        //recalculate angles if bigger than 180
        float angleX, angleY, angleZ;
        angleX = filteredAngularSpeed.x;
        angleY = filteredAngularSpeed.y;
        angleZ = filteredAngularSpeed.z;


        if (angleX > 180)
        {
            angleX -= 360;
        }
        if (angleY > 180)
        {
            angleY -= 360;
        }
        if (angleZ > 180)
        {
            angleZ -= 360;
        }

        filteredAngularSpeed = new Vector3(angleX,angleY,angleZ)/Time.fixedDeltaTime;

        rot_i_1 = rot_i;
    }

    public void SetParent()
    {
        objectInHand.transform.SetParent(transform);

    }

    public void EnableColliders(bool b)
    {
        for (int ii = 0; ii < colliders.Length; ii++)
        {
            colliders[ii].enabled = b;
        }
    }
}
