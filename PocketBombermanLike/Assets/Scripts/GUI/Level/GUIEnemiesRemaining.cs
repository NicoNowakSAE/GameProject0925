using TMPro;
using UnityEngine;

public class GUIEnemiesRemaining : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _enemiesRemainingLabel;

    private void Update()
    {
        _enemiesRemainingLabel.text = LevelController.Instance.EnemiesRemaining.ToString();
    }
}
