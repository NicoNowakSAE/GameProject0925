using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIHeartBar : MonoBehaviour
{
    [SerializeField] private Sprite _baseHeart;
    [SerializeField] private Sprite _disabledHeart;
    [SerializeField] private List<Image> _spriteObjects;
    private int _maxHearts;

    private void Awake()
    {
        _maxHearts = LevelController.Instance.HeartsCount;

        foreach (Image image in _spriteObjects)
            image.sprite = _baseHeart;
    }

    private void Start()
    {
        if (LevelController.Instance.PlayerHealth != null) 
            LevelController.Instance.PlayerHealth.OnEntityDeath.AddListener(RefreshHearts);
    }

    private void RefreshHearts()
    {
        Debug.Log($"[GUI HEART BAR] Heart refresh invoked -");

        if (_spriteObjects.Count <= 0)
            return;

        if (LevelController.Instance.HeartsCount >= 0)
        {
            for (int i = 0; i < _maxHearts; i++)
            {
                if (i < LevelController.Instance.HeartsCount)
                {
                    _spriteObjects[i].sprite = _baseHeart;
                    Debug.Log($"[GUI HEART BAR] Setting sprite: {_spriteObjects[i].gameObject.name} to Base Heart -");
                }
                else
                {
                    _spriteObjects[i].sprite = _disabledHeart;
                    Debug.Log($"[GUI HEART BAR] Setting sprite: {_spriteObjects[i].gameObject.name} to Disabled Heart -");
                }
            }
        }


    }
}
