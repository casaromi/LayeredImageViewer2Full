using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicRig : MonoBehaviour 
{ 

	float moveSpeed = 3.0f;
	float turnSpeed = 50.0f;

	CharacterController cc;
    // Start is called before the first frame update
    void Start()
    {
		cc= GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {
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
			transform.Rotate(transform.right, turnSpeed * lookUp * Time.deltaTime, Space.World);
		}
		float roll = Input.GetAxis("Roll");
		transform.Rotate(transform.forward, -0.1f*turnSpeed * roll * Time.deltaTime, Space.World);


	}
}
