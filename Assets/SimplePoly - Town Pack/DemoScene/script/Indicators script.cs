using UnityEngine;
using UnityEngine.UI;

public class IndicatorLights : MonoBehaviour
{
    // Reference to the left and right indicator lights
    public GameObject leftIndicator;
    public GameObject rightIndicator;

    // Reference to the left and right buttons
    public Button leftButton;
    public Button rightButton;

    // Color to use for the indicator lights
    public Color indicatorColor = Color.yellow;

    private bool isLeftButtonPressed = false;
    private bool isRightButtonPressed = false;

    private void Start()
    {
        // Initialize the indicator lights to be off
        leftIndicator.GetComponent<Renderer>().material.color = Color.yellow;
        rightIndicator.GetComponent<Renderer>().material.color = Color.yellow;

        // Add event listeners to the buttons
        leftButton.onClick.AddListener(LeftButtonClicked);
        rightButton.onClick.AddListener(RightButtonClicked);
    }

    private void LeftButtonClicked()
    {
        isLeftButtonPressed = true;
        leftIndicator.GetComponent<Renderer>().material.color = indicatorColor;
    }

    private void RightButtonClicked()
    {
        isRightButtonPressed = true;
        rightIndicator.GetComponent<Renderer>().material.color = indicatorColor;
    }

    private void Update()
    {
        if (!isLeftButtonPressed)
        {
            leftIndicator.GetComponent<Renderer>().material.color = Color.white;
        }

        if (!isRightButtonPressed)
        {
            rightIndicator.GetComponent<Renderer>().material.color = Color.white;
        }
    }

    public void LeftButtonReleased()
    {
        isLeftButtonPressed = false;
    }

    public void RightButtonReleased()
    {
        isRightButtonPressed = false;
    }
}