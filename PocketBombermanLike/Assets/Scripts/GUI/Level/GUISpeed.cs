using TMPro;
using UnityEngine;

public class GUISpeed : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _speedLabel;

    private void Update()
    {
        _speedLabel.text = PlayerStatsSystem.Instance.GetStats.Speed.ToString();
    }
}
