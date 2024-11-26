using UnityEngine;

public class CarColorChanger : MonoBehaviour
{
    public Renderer CarRenderer; // Assign your car's Renderer component here.

    private void Start()
    {
        if (ColorManager.Instance != null)
        {
            // Apply the selected color to the car material
            CarRenderer.material.color = ColorManager.Instance.SelectedColor;
        }
    }
}
