using TMPro;
using UnityEngine;

public class GUIBombRange : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _bombRangeLabel;

    private void Update()
    {
        _bombRangeLabel.text = PlayerStatsSystem.Instance.GetStats.BombRange.ToString();
    }
}
