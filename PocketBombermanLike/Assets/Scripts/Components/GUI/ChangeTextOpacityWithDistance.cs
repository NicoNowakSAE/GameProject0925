using TMPro;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Rendering;

public class ChangeTextOpacityWithDistance : MonoBehaviour
{
    private Transform _playerTransform;
    private Transform _transform;
    [SerializeField] private TextMeshPro _textMesh;
    [SerializeField] private float _min;
    [SerializeField] private float _max;

    private void Start()
    {
        _playerTransform = LevelController.Instance.Player.GetComponent<Transform>();
        _transform = GetComponent<Transform>();
    }

    private void Update()
    {
        float distance = (_transform.position - _playerTransform.position).sqrMagnitude;
        float opacity = (Mathf.Sqrt(distance) - _max) / (_min - _max);
        Color color = _textMesh.color;
        color.a = Mathf.Clamp(opacity, _min, _max);
        _textMesh.color = color;
    }
}
