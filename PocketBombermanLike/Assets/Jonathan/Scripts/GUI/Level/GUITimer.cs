using System;
using TMPro;
using UnityEngine;

public class GUITimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _timerLabel;
    private LevelController _levelController;

    private void Awake()
    {
        _levelController = FindFirstObjectByType<LevelController>();
    }

    private void Update()
    {
        TimeSpan timeElapsed = _levelController.TimeElapsed;
        _timerLabel.text = $"{String.Format("{0:D2}", timeElapsed.Minutes)}:{String.Format("{0:D2}", timeElapsed.Seconds)}";
    }
}
