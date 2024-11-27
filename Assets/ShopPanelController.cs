using UnityEngine;

public class ShopPanelController : MonoBehaviour
{
    public void SetRedColor()
    {
        ChangeColor(Color.red);
    }

    public void SetBlueColor()
    {
        ChangeColor(Color.blue);
    }

    public void SetGreenColor()
    {
        ChangeColor(Color.green);
    }

    private void ChangeColor(Color newColor)
    {
        if (ColorManager.Instance != null)
        {
            ColorManager.Instance.SelectedColor = newColor;
        }
    }
}
