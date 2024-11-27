using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public string gameplaySceneName = "DemoScene"; // Set this to the name of your gameplay scene

    public void OnPlayButtonClicked()
    {
        // Load the gameplay scene
        SceneManager.LoadScene(gameplaySceneName);
    }
}
