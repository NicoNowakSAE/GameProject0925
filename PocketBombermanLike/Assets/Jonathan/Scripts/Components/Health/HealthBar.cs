using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health _trackObject;
    private Slider _hpBarSlider;

    private void Awake()
    {
        _hpBarSlider = GetComponent<Slider>();
    }

    public void UpdateHpBar()
    {
        float targetValue = _trackObject.CurrentHealth / _trackObject.BaseHealth;

        _hpBarSlider.value = targetValue;
    }

    private void Update()
    {
        UpdateHpBar();
    }
}
