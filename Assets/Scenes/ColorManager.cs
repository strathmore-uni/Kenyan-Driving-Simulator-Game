using UnityEngine;

public class ColorManager : MonoBehaviour
{
    public static ColorManager Instance;
    public Color SelectedColor = Color.white; // Default color

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Avoid duplicates
        }
    }
}
