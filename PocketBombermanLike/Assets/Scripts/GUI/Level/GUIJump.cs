using TMPro;
using UnityEngine;

public class GUIJump : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _jumpLabel;

    private void Update()
    {
        _jumpLabel.text = PlayerStatsSystem.Instance.GetStats.JumpForce.ToString();
    }
}
