using UnityEngine;

public class CarCustomization : MonoBehaviour
{
    public Renderer carRenderer;       // Reference to the car's renderer
    public Material[] paintMaterials; // Array of materials for car paints

    public void ApplyPaint(int index)
    {
        if (index >= 0 && index < paintMaterials.Length)
        {
            carRenderer.material = paintMaterials[index]; // Change the car color
            PlayerPrefs.SetInt("SelectedPaint", index);   // Save the selection
            PlayerPrefs.Save(); // Write data to disk
        }
    }
}
