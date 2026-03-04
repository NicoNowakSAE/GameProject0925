using TMPro;
using UnityEngine;

public class Panel : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _headerLabel;
    [SerializeField] private string _headerText;
    [SerializeField] private TextMeshProUGUI _contentLabel;
    [SerializeField] private string _contentText;
    [SerializeField] private TextMeshProUGUI _buttonProceedLabel;
    [SerializeField] private string _buttonProceedText;
    [SerializeField] private TextMeshProUGUI _buttonDiscardLabel;
    [SerializeField] private string _buttonDiscardText;

    public void SetHeaderText(string text)
    {
        _headerText = text;
        _headerLabel.text = _headerText;
    }

    public void SetContentText(string text)
    {
        _contentText = text;
        _contentLabel.text = _contentText;
    }

    public void SetButtonProceedText(string text)
    {
        _buttonProceedText = text;
        _buttonProceedLabel.text = _buttonProceedText;
    }

    public void SetButtonDiscardText(string text)
    {
        _buttonDiscardText = text;
        _buttonDiscardLabel.text = _buttonDiscardText;
    }
    
    private void InitContent()
    {
        _headerLabel.text = _headerText;
        _contentLabel.text = _contentText;
        _buttonProceedLabel.text = _buttonProceedText;
        _buttonDiscardLabel.text = _buttonDiscardText;
    }

    private void OnEnable()
    {
        InitContent();
    }
}
