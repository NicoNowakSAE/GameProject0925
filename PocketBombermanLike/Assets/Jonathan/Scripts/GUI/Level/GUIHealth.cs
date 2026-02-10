using TMPro;
using UnityEngine;

public class GUIHealth : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _healthLabel;
    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindFirstObjectByType<LevelController>();
    }

    private void Update()
    {
        _healthLabel.text = _levelController.PlayerHealth.ToString();
    }
}
