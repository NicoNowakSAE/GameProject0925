using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class SwitchButton : MonoBehaviour
{
    [Header("Disabled Alpha For Button Label")]
    [Range(0, 255)]
    [SerializeField] private float _disabledButtonLabelAlpha = 25;

    [Header("Set Active State")]
    [SerializeField] private bool _isActive = true;

    private bool _lastActiveState = true;
    private Button _button;
    private TextMeshProUGUI _buttonLabel;
    private Color _baseButtonLabelColor;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _buttonLabel = GetComponentInChildren<TextMeshProUGUI>();

        if (_buttonLabel != null)
            _baseButtonLabelColor = _buttonLabel.color;
    }

    private void Update()
    {
        if (_lastActiveState != _isActive)
        {
            SetActive(_isActive);
            _lastActiveState = _isActive;
        }
    }
    public void SetActive(bool value)
    {
        Debug.Log($"[BUTTON SWITCH] Changing state to: {value} -");

        _isActive = value;
        _button.interactable = _isActive;

        if (_buttonLabel == null)
            return;

        if (!_isActive)
        {
            _buttonLabel.color = new Color(
                r: _baseButtonLabelColor.r,
                g: _baseButtonLabelColor.g,
                b: _baseButtonLabelColor.b,
                a: _disabledButtonLabelAlpha / 255.0f
            );

            return;
        }

        _buttonLabel.color = _baseButtonLabelColor;

    }
}