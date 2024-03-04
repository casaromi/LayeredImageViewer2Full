using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenChat : MonoBehaviour
{
    public GameObject ChatPanel;

    private void Start()
    {
        // Ensure the ChatPanel is initially disabled
        ChatPanel.SetActive(false);
    }

    private void Update()
    {
        // Check for the "F1" key press
        if (Input.GetKeyDown(KeyCode.F1))
        {
            // Toggle the leaveGamePanel on or off
            ChatPanel.SetActive(!ChatPanel.activeSelf);


            // Pause or resume the game based on the ChatPanel's active state
            // Optionally, you can also pause the game when the menu opens
            Time.timeScale = (ChatPanel.activeSelf) ? 0f : 1f;
        }
    }

}
