using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRig : MonoBehaviour
{

	float moveSpeed = 10.0f;
	float turnSpeed = 1000.0f;

	CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
		cc= GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
		/* This is client code
		if (!networkObject.IsServer)
		{
			transform.position = networkObject.position;
			transform.rotation = networkObject.rotation;
			return;
		}
		*/

		// This is server code
		float forward = Input.GetAxis("Vertical");
		float side = Input.GetAxis("Horizontal");
		float up = Input.GetAxis("Jump");
		if(Input.GetKey(KeyCode.LeftShift)|| Input.GetKey(KeyCode.RightShift))
		{
			up = -up;
		}

		cc.Move(moveSpeed* (forward*transform.forward+side*transform.right + up * transform.up) *Time.deltaTime);

		if(Input.GetMouseButton(1)) { 
			float twist = Input.GetAxis("Mouse X");
			float lookUp = -Input.GetAxis("Mouse Y");
			transform.Rotate(transform.up, turnSpeed * twist * Time.deltaTime, Space.World);
			transform.Rotate(transform.right, turnSpeed * lookUp * Time.deltaTime, Space.Self);
		}

		//networkObject.position = transform.position;
		//networkObject.rotation = transform.rotation;
	}
}
