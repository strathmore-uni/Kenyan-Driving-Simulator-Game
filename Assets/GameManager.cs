using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private bool isPaused = false;

    // Pause the game
    public void PauseGame()
    {
        if (!isPaused)
        {
            Time.timeScale = 0;  // Pauses the game
            AudioListener.pause = true;  // Pauses all audio
            isPaused = true;
        }
        else
        {
            Time.timeScale = 1;  // Resumes the game
            AudioListener.pause = false;  // Resumes all audio
            isPaused = false;
        }
    }


    // Replay the current level
    public void ReplayGame()
    {
        Time.timeScale = 1;  // Ensures the game is unpaused
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);  // Reloads the current scene
    }

    // Go back to the main menu
    public void BackToMainMenu()
    {
        Time.timeScale = 1;  // Ensures the game is unpaused
        SceneManager.LoadScene("MainMenu");  // Loads the MainMenu scene (make sure the MainMenu scene is added to your build settings)
    }
}
