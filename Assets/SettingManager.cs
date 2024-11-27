using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    // References to the setting panels
    [SerializeField] private GameObject mainMenuPanel;        // Main settings menu panel
    [SerializeField] private GameObject soundSettingsPanel;   // Sound settings panel
    [SerializeField] private GameObject graphicsSettingsPanel; // Graphics settings panel
    [SerializeField] private GameObject controlsSettingsPanel; // Controls settings panel

    // Set all panels inactive initially
    private void Start()
    {
        ShowMainMenu(); // Start by showing the main menu
    }

    // Show the Main Settings Menu
    public void ShowMainMenu()
    {
        mainMenuPanel.SetActive(true);
        soundSettingsPanel.SetActive(false);
        graphicsSettingsPanel.SetActive(false);
        controlsSettingsPanel.SetActive(false);
    }

    // Show Sound Settings
    public void ShowSoundSettings()
    {
        mainMenuPanel.SetActive(false);
        soundSettingsPanel.SetActive(true);
        graphicsSettingsPanel.SetActive(false);
        controlsSettingsPanel.SetActive(false);
    }

    // Show Graphics Settings
    public void ShowGraphicsSettings()
    {
        mainMenuPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
        graphicsSettingsPanel.SetActive(true);
        controlsSettingsPanel.SetActive(false);
    }

    // Show Controls Settings
    public void ShowControlsSettings()
    {
        mainMenuPanel.SetActive(false);
        soundSettingsPanel.SetActive(false);
        graphicsSettingsPanel.SetActive(false);
        controlsSettingsPanel.SetActive(true);
    }
}
