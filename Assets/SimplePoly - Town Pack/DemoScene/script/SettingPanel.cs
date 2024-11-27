using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingPanel : MonoBehaviour
{
    public GameObject mainMenuPanel;   // Reference to the main menu panel
    public GameObject settingsPanel;   // Reference to the settings panel

    void Start()
    {
        // Ensure the main menu is visible, and settings panel is hidden at the start
        mainMenuPanel.SetActive(true);
        settingsPanel.SetActive(false);
    }

    public void OnSettingsButtonClicked()
    {
        // Show the settings panel and hide the main menu
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void OnBackButtonClicked()
    {
        // Hide the settings panel and show the main menu
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
}
