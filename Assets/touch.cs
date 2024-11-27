using UnityEngine;

public class TouchInputTest : MonoBehaviour
{
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Debug.Log($"Touch Position: {touch.position}"); // Log touch position
        }
    }
}
