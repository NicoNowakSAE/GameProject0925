using TMPro;
using UnityEngine;

public class GUICurrentLevel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currentLevelLabel;
    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindFirstObjectByType<LevelController>();
    }

    private void Update()
    {
        _currentLevelLabel.text = _levelController.CurrentLevel.ToString();
    }
}
