using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthTester : MonoBehaviour
{
    [Serializable]
    public enum reduceIncreaseValue
    {
        Reduce,
        Increase,
        Kill
    }

    [Serializable]
    public struct HealthTestPair
    {
        public KeyCode ActionKey;
        public reduceIncreaseValue Action;
        public float Value;
    }

    [SerializeField] private List<HealthTestPair> _keyMap;
    [SerializeField] private Health _testHealthObject;

    void Update()
    {
        foreach (HealthTestPair valuePair in _keyMap)
        {
            if (Input.GetKeyDown(valuePair.ActionKey))
            {
                _testHealthObject.Reduce(valuePair.Value);
            }
        }
    }
}
