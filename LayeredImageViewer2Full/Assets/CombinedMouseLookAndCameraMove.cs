/*
using UnityEngine;

public class CombinedMouseLookAndCameraMove : MonoBehaviour
{
    float mainSpeed = 5.0f; // regular speed
    float shiftAdd = 10.0f; // multiplied by how long shift is held. Basically running
    float maxShift = 15.0f; // Maximum speed when holding shift
    float camSens = 0.2f; // How sensitive it is with the mouse

    private Vector3 lastMouse; // previous mouse position
    private float totalRun = 1.0f;

    private void Start()
    {
        lastMouse = Input.mousePosition;
    }

    private void Update()
    {
        // Mouse camera angle
        Vector3 mouseDelta = Input.mousePosition - lastMouse;
        lastMouse = Input.mousePosition;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= mouseDelta.y * camSens;
        rotation.y += mouseDelta.x * camSens;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        // Keyboard commands
        Vector3 movement = GetBaseInput();

        // Only move while a direction key is pressed
        if (movement.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                movement *= totalRun * shiftAdd;
                movement.x = Mathf.Clamp(movement.x, -maxShift, maxShift);
                movement.y = Mathf.Clamp(movement.y, -maxShift, maxShift);
                movement.z = Mathf.Clamp(movement.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 1f, 2f, 1000f);
                movement *= mainSpeed;
            }

            movement *= Time.deltaTime;
            Vector3 newPosition = transform.position;

            // If player wants to move up and down
            if (!Input.GetKey(KeyCode.P))
            {
                transform.Translate(movement);
                newPosition.x = transform.position.x;
                newPosition.z = transform.position.z;
                transform.position = newPosition;
            }
            else
            {
                transform.Translate(movement);
            }
        }
    }

    // Returns the basic movement values
    private Vector3 GetBaseInput()
    {
        Vector3 velocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(1, 0, 0);
        }

        return velocity;
    }
}
*/



















/*
using UnityEngine;

public class CombinedMouseLookAndCameraMove : MonoBehaviour
{
    float mainSpeed = 5.0f; // regular speed
    float shiftAdd = 8.0f; // multiplied by how long shift is held. Basically running
    float maxShift = 10.0f; // Maximum speed when holding shift
    float camSens = 0.2f; // How sensitive it is with the mouse

    private Vector3 lastMouse; // previous mouse position
    private float totalRun = 1.0f;

    private CharacterController characterController;

    private void Start()
    {
        lastMouse = Input.mousePosition;
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // Mouse camera angle
        Vector3 mouseDelta = Input.mousePosition - lastMouse;
        lastMouse = Input.mousePosition;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= mouseDelta.y * camSens;
        rotation.y += mouseDelta.x * camSens;
        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        // Keyboard commands
        Vector3 movement = GetBaseInput();

        // Only move while a direction key is pressed
        if (movement.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                movement *= totalRun * shiftAdd;
                movement.x = Mathf.Clamp(movement.x, -maxShift, maxShift);
                movement.z = Mathf.Clamp(movement.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 1f, 2f, 1000f);
                movement *= mainSpeed;
            }

            movement *= Time.deltaTime;

            // Set the y component of the movement vector to 0
            movement.y = 0f;

            // Get the player's forward direction without considering the y-axis
            Vector3 forwardDirection = transform.forward;
            forwardDirection.y = 0f;
            forwardDirection.Normalize();

            // Get the player's right direction without considering the y-axis
            Vector3 rightDirection = transform.right;
            rightDirection.y = 0f;
            rightDirection.Normalize();

            // Calculate the movement vector in the x-z plane
            Vector3 movementXZ = (forwardDirection * movement.z) + (rightDirection * movement.x);

            // Move the character using the Character Controller component
            characterController.Move(movementXZ);
        }
    }

    // Returns the basic movement values
    private Vector3 GetBaseInput()
    {
        Vector3 velocity = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(1, 0, 0);
        }

        return velocity.normalized;
    }
}

*/














using UnityEngine;

public class CombinedMouseLookAndCameraMove : MonoBehaviour
{
    float mainSpeed = 5.0f; // regular speed
    float shiftAdd = 10.0f; // multiplied by how long shift is held. Basically running
    float maxShift = 15.0f; // Maximum speed when holding shift
    float camSens = 0.2f; // How sensitive it is with the mouse

    private Vector3 lastMouse; // previous mouse position
    private float totalRun = 1.0f;

    private void Start()
    {
        lastMouse = Input.mousePosition;
    }

    private void Update()
    {
        // Mouse camera angle
        Vector3 mouseDelta = Input.mousePosition - lastMouse;
        lastMouse = Input.mousePosition;

        Vector3 rotation = transform.rotation.eulerAngles;
        rotation.x -= mouseDelta.y * camSens;
        rotation.y += mouseDelta.x * camSens;
        rotation.z = 0;
        transform.rotation = Quaternion.Euler(rotation);

        // Keyboard commands
        Vector3 movement = GetBaseInput();

        // Only move while a direction key is pressed
        if (movement.sqrMagnitude > 0)
        {
            if (Input.GetKey(KeyCode.LeftShift))
            {
                totalRun += Time.deltaTime;
                movement *= totalRun * shiftAdd;
                movement.x = Mathf.Clamp(movement.x, -maxShift, maxShift);
                movement.y = Mathf.Clamp(movement.y, -maxShift, maxShift);
                movement.z = Mathf.Clamp(movement.z, -maxShift, maxShift);
            }
            else
            {
                totalRun = Mathf.Clamp(totalRun * 1f, 2f, 1000f);
                movement *= mainSpeed;
            }

            movement *= Time.deltaTime;
            Vector3 newPosition = transform.position;

            // If player wants to move up and down
            if (!Input.GetKey(KeyCode.P))
            {
                // Calculate the target position
                Vector3 targetPosition = transform.position + movement;

                // Perform a collision check before moving
                if (!IsColliding(targetPosition, out Vector3 adjustedMovement))
                {
                    transform.Translate(adjustedMovement);
                    newPosition.x = transform.position.x;
                    newPosition.z = transform.position.z;
                    transform.position = newPosition;
                }
            }
            else
            {
                transform.Translate(movement);
            }
        }
    }

    // Returns the basic movement values
    private Vector3 GetBaseInput()
    {
        Vector3 velocity = new Vector3();

        if (Input.GetKey(KeyCode.W))
        {
            velocity += new Vector3(0, 0, 1);
        }

        if (Input.GetKey(KeyCode.S))
        {
            velocity += new Vector3(0, 0, -1);
        }

        if (Input.GetKey(KeyCode.A))
        {
            velocity += new Vector3(-1, 0, 0);
        }

        if (Input.GetKey(KeyCode.D))
        {
            velocity += new Vector3(1, 0, 0);
        }

        return velocity.normalized;
    }

    // Check for collision with objects that have box colliders
    private bool IsColliding(Vector3 targetPosition, out Vector3 adjustedMovement)
    {
        Collider[] colliders = Physics.OverlapSphere(targetPosition, 0.5f); // Use an appropriate radius

        adjustedMovement = Vector3.zero;
        bool isColliding = false;

        foreach (Collider collider in colliders)
        {
            // Check if the collider has a box collider
            BoxCollider boxCollider = collider.GetComponent<BoxCollider>();
            if (boxCollider != null)
            {
                isColliding = true;
                break;
            }
        }

        if (!isColliding)
        {
            adjustedMovement = targetPosition - transform.position;
        }

        return isColliding;
    }
}
