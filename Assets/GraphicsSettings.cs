using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class GraphicsSettingsManager : MonoBehaviour
{
    public TMP_Dropdown qualityDropdown;
    public TMP_Dropdown resolutionDropdown;

    private Resolution[] availableResolutions;

    void Start()
    {
        // Retrieve available screen resolutions
        availableResolutions = Screen.resolutions;

        // Clear and populate resolution dropdown options
        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < availableResolutions.Length; i++)
        {
            string option = availableResolutions[i].width + "x" + availableResolutions[i].height;
            options.Add(option);

            // Check if this resolution matches the current screen resolution
            if (availableResolutions[i].width == Screen.width && availableResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        // Add listener to apply resolution change when dropdown selection is changed
        resolutionDropdown.onValueChanged.AddListener(SetResolution);
    }

    // Set quality level based on dropdown selection
    public void SetQuality(int index)
    {
        QualitySettings.SetQualityLevel(index);
    }

    // Set resolution based on dropdown selection
    public void SetResolution(int index)
    {
        Resolution chosenResolution = availableResolutions[index];
        Screen.SetResolution(chosenResolution.width, chosenResolution.height, Screen.fullScreen);
    }
}
