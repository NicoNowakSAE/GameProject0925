using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SwitchButton))]
public class GUIButton_Continue : MonoBehaviour
{
    private Button _button;
    private SwitchButton _switchButton; 

    private void Awake()
    {
        _button = GetComponent<Button>();
        _switchButton = GetComponent<SwitchButton>();
    }

    private void Start()
    {
        TryFetchLoadData();
    }

    private void TryFetchLoadData()
    {
        SaveData data = SaveLoadSystem.Load();
        _switchButton.SetActive(data != null);
    }
}
