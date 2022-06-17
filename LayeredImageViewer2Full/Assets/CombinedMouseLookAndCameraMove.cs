using UnityEngine;
using System.Collections;

public class CombinedMouseLookAndCameraMove : MonoBehaviour
{
    //WASD: Basic movement
    //Shift: Makes camera accelerate
    //P: Moves camera up and down; camera gains height


	float mainSpeed = 5.0f; //regular speed
	float shiftAdd = 10.0f; //multiplied by how long shift is held.  Basically running
	float maxShift = 15.0f; //Maximum speed when holdin gshift
	float camSens = 0.2f; //How sensitive it with mouse


	//kind of in the middle of the screen, rather than at the top (play)
	private Vector3 lastMouse = new Vector3(255, 255, 255); 
	private float totalRun = 1.0f;

	void Update()
	{
		//Mouse camera angle 
		lastMouse = Input.mousePosition - lastMouse;
		lastMouse = new Vector3(-lastMouse.y * camSens, lastMouse.x * camSens, 0);
		lastMouse = new Vector3(transform.eulerAngles.x + lastMouse.x, transform.eulerAngles.y + lastMouse.y, 0);
		transform.eulerAngles = lastMouse;
		lastMouse = Input.mousePosition;
		  

		//Keyboard commands
		//float f = 0.0f;
		Vector3 p = GetBaseInput();

		//Only move while a direction key is pressed
		if (p.sqrMagnitude > 0)
		{ 
			if (Input.GetKey(KeyCode.LeftShift))
			{
				totalRun += Time.deltaTime;
				p = p * totalRun * shiftAdd;
				p.x = Mathf.Clamp(p.x, -maxShift, maxShift);
				p.y = Mathf.Clamp(p.y, -maxShift, maxShift);
				p.z = Mathf.Clamp(p.z, -maxShift, maxShift);
			}

			else
			{
				totalRun = Mathf.Clamp(totalRun * 1f, 2f, 1000f);
				p = p * mainSpeed;
			}

			p = p * Time.deltaTime;
			Vector3 newPosition = transform.position;


			//If player wants to move up and down
			if (!Input.GetKey(KeyCode.P))
			{ 
				transform.Translate(p);
				newPosition.x = transform.position.x;
				newPosition.z = transform.position.z;
				transform.position = newPosition;
			}

			else
			{
				transform.Translate(p);
			}

		}
	}


	//returns the basic values, if it's 0 than it's not active.
	private Vector3 GetBaseInput()
	{ 
		Vector3 p_Velocity = new Vector3();

		if (Input.GetKey(KeyCode.W))
		{
			p_Velocity += new Vector3(0, 0, 1);
		}

		if (Input.GetKey(KeyCode.S))
		{
			p_Velocity += new Vector3(0, 0, -1);
		}

		if (Input.GetKey(KeyCode.A))
		{
			p_Velocity += new Vector3(-1, 0, 0);
		}

		if (Input.GetKey(KeyCode.D))
		{
			p_Velocity += new Vector3(1, 0, 0);
		}

		return p_Velocity;
	}

}
