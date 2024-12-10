using UnityEngine;
using UnityEngine.SceneManagement; // For loading scenes

public class CarShopManager : MonoBehaviour
{
    public void SelectCarAndStartGame(int carIndex)
    {
        // Set the selected car in the CarSelectionManager
        CarSelectionManager.Instance.SelectCar(carIndex);

        // Load the gameplay scene
        SceneManager.LoadScene("DemoScene"); // Replace with your gameplay scene name
    }
}
