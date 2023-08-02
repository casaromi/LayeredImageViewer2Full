using UnityEngine;

public class LeaveGameMenu : MonoBehaviour
{
    public GameObject leaveGamePanel;

    private void Start()
    {
        // Ensure the leaveGamePanel is initially disabled
        leaveGamePanel.SetActive(false);
    }

    private void Update()
    {
        // Check for the "Escape" key press
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Toggle the leaveGamePanel on or off
            leaveGamePanel.SetActive(!leaveGamePanel.activeSelf);

            // Optionally, you can also pause the game when the menu opens
            Time.timeScale = (leaveGamePanel.activeSelf) ? 0f : 1f;
        }
    }

    public void OnConfirmLeaveGame()
    {
        // Implement code to leave the game here
        Debug.Log("Leave game confirmed.");

        Application.Quit();
    }

    public void OnCancelLeaveGame()
    {
        // Hide the leaveGamePanel when the user cancels
        leaveGamePanel.SetActive(false);

        // Optionally, resume the game when the menu is closed
        Time.timeScale = 1f;
    }
}
