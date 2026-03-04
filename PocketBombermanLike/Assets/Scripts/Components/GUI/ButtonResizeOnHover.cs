using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class ButtonResizeOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _onHoverSizeMultiplier = 1.075f;
    [SerializeField] private float _sizeChangeDuration = 0.75f;
    [SerializeField] private AnimationCurve _curveBehaviour;
    private Vector3 _baseSize;
    private Button _button;
    private bool _isCoroutineActive = false;

    private enum SizeChangeMode
    {
        Increase,
        Decrease
    }

    private void ResetSize() => _button.transform.localScale = _baseSize;
    private void OnEnable() => ResetSize();
    
    private IEnumerator ChangeSize(SizeChangeMode mode)
    {
        _isCoroutineActive = true;

        Vector3 startScale = _button.transform.localScale;
        Vector3 targetScale = _baseSize;

        switch (mode)
        {
            case SizeChangeMode.Increase:
                targetScale = startScale * _onHoverSizeMultiplier;
                break;

            case SizeChangeMode.Decrease:
                targetScale = _baseSize;
                break;
        }

        float currentTime = 0.0f;

        while (currentTime < _sizeChangeDuration)
        {
            currentTime += Time.deltaTime;
            float normalizedTime = currentTime / _sizeChangeDuration;

            _button.transform.localScale = Vector3.Lerp(
                _button.transform.localScale,
                targetScale,
                _curveBehaviour.Evaluate(normalizedTime)
            );

            yield return new WaitForEndOfFrame();

        }

        _isCoroutineActive = false;

        yield return null;
    }

    private void Awake()
    {
        _button = GetComponent<Button>();
        _baseSize = _button.transform.localScale;
        
        if (_curveBehaviour == null)
        {
            Debug.LogWarning("[BUTTON RESIZE ON HOVER] Button {gameObject.name} has no OnHover animation curve, therefore it won't animate -");
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_isCoroutineActive == true)
            StopAllCoroutines();

        StartCoroutine(ChangeSize(mode: SizeChangeMode.Increase));
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_isCoroutineActive == true)
            StopAllCoroutines();

        StartCoroutine(ChangeSize(mode: SizeChangeMode.Decrease));
    }
}
