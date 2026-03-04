using TMPro;
using UnityEngine;

public class GUIBombCount : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombCountLabel;

    private void Update()
    {
        _bombCountLabel.text = PlayerStatsSystem.Instance.GetStats.BombCount.ToString();
    }
}
