using System;
using TMPro;
using UnityEngine;

public class GUICountdown : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _countdownLabel;

    private void Update()
    {
        TimeSpan timeLeft = LevelController.Instance.LevelTimeLeft;

        if (_countdownLabel != null && timeLeft != null)
        {
            _countdownLabel.text = $"{String.Format("{0:D2}", timeLeft.Minutes)}:{String.Format("{0:D2}", timeLeft.Seconds)}";
        }
        
    }
}
