using UnityEngine;

public class GameDoneController : MonoBehaviour
{
    private void Start()
    {
        GUIController.Instance.OpenWinMenu();
    }
}
