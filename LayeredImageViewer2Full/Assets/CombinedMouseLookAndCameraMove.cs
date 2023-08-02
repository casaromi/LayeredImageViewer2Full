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
                if (!IsColliding(targetPosition, movement, out Vector3 adjustedMovement))
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
    private bool IsColliding(Vector3 targetPosition, Vector3 movement, out Vector3 adjustedMovement)
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
                // Check if the collider is blocking the movement direction
                Vector3 direction = movement.normalized;
                if (Vector3.Dot(direction, collider.transform.forward) < 0)
                {
                    isColliding = true;
                    break;
                }
            }
        }

        if (!isColliding)
        {
            adjustedMovement = targetPosition - transform.position;
        }

        return isColliding;
    }
}
*/







using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class CombinedMouseLookAndCameraMove : MonoBehaviour
{
    float mainSpeed = 5.0f; // regular speed
    float shiftAdd = 10.0f; // multiplied by how long shift is held. Basically running
    float maxShift = 15.0f; // Maximum speed when holding shift
    float camSens = 0.2f; // How sensitive it is with the mouse

    private Vector3 lastMouse; // previous mouse position
    private float totalRun = 1.0f;
    private bool isInputFieldSelected = false;
    private LeaveGameMenu leaveGameMenu; // Reference to the LeaveGameMenu script

    private void Start()
    {
        lastMouse = Input.mousePosition;

        // Find the LeaveGameMenu script in the scene
        leaveGameMenu = FindObjectOfType<LeaveGameMenu>();

        if (leaveGameMenu == null)
        {
            Debug.LogWarning("CombinedMouseLookAndCameraMove: LeaveGameMenu script not found in the scene!");
        }
    }

    private void Update()
    {
        // Check if an input field is currently selected
        if (EventSystem.current.currentSelectedGameObject != null &&
            EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null)
        {
            // An input field is selected
            isInputFieldSelected = true;
        }
        else
        {
            // No input field is selected
            isInputFieldSelected = false;
        }

        // Check if the leave game menu is active
        bool isLeaveGameMenuActive = (leaveGameMenu != null) ? leaveGameMenu.leaveGamePanel.activeSelf : false;

        // Mouse camera angle
        if (!isLeaveGameMenuActive) // Only update the rotation if the leave game menu is not active
        {
            Vector3 mouseDelta = Input.mousePosition - lastMouse;
            lastMouse = Input.mousePosition;

            Vector3 rotation = transform.rotation.eulerAngles;
            rotation.x -= mouseDelta.y * camSens;
            rotation.y += mouseDelta.x * camSens;
            rotation.z = 0;
            transform.rotation = Quaternion.Euler(rotation);
        }

        // Keyboard commands
        Vector3 movement = GetBaseInput();

        // Only move while a direction key is pressed and no input field is selected
        if (movement.sqrMagnitude > 0 && !isInputFieldSelected && !isLeaveGameMenuActive)
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
                if (!IsColliding(targetPosition, movement, out Vector3 adjustedMovement))
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
    private bool IsColliding(Vector3 targetPosition, Vector3 movement, out Vector3 adjustedMovement)
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
                // Check if the collider is blocking the movement direction
                Vector3 direction = movement.normalized;
                if (Vector3.Dot(direction, collider.transform.forward) < 0)
                {
                    isColliding = true;
                    break;
                }
            }
        }

        if (!isColliding)
        {
            adjustedMovement = targetPosition - transform.position;
        }

        return isColliding;
    }
}
