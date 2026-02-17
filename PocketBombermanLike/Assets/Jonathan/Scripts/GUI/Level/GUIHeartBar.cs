using System.Collections.Generic;
using System.Security.Cryptography;
using Microsoft.Unity.VisualStudio.Editor;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

public class HeartBar : MonoBehaviour
{
    [SerializeField] private Sprite _spriteAsset;
    private RectTransform _rectTransform;
    private List<GameObject> _heartBarImages = new List<GameObject>();
    [SerializeField] private int _amount;
    [SerializeField] private float _padding;

    [SerializeField] private int testhp;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
    }

    private void Start()
    {
        for (int i = 0; i < _amount; i++)
        {
            GameObject newObj = new GameObject("HeartImage");

            newObj.AddComponent<UnityEngine.UI.Image>();
            newObj.AddComponent<UnityEngine.CanvasRenderer>();
            newObj.AddComponent<UnityEngine.RectTransform>();

            newObj.transform.SetParent(this.gameObject.transform);

            RectTransform newObjRectTransform = newObj.GetComponent<RectTransform>();
            UnityEngine.UI.Image newObjImage = newObj.GetComponent<UnityEngine.UI.Image>();

            newObjImage.sprite = _spriteAsset;

            float newObjWidth = 32;
            float newObjHeight = 32;

            newObjRectTransform.sizeDelta = new Vector2(newObjWidth, newObjHeight);

            float totalWidth = _amount * newObjWidth + (_amount - 1) * Mathf.Max(1, _padding);
            float startX = -totalWidth / 2 + newObjWidth / 2;

            newObjRectTransform.anchoredPosition = new Vector2(
                startX + i * (newObjWidth + _padding),
                0
            );
            _heartBarImages.Add(newObj);
        }
    }

    private void Update()
    {
        for (int i = 0; i < _heartBarImages.Count; i++)
        {
            _heartBarImages[i].SetActive(true);
            if (i >= testhp)
            {
                _heartBarImages[i].SetActive(false);
            }
        }
    }
}
