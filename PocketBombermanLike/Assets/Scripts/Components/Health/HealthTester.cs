using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class HealthTester : MonoBehaviour
{
    [Serializable]
    public enum ReduceIncreaseValue
    {
        Reduce,
        Increase
    }

    [Serializable]
    public struct HealthTestPair
    {
        public KeyCode ActionKey;
        public ReduceIncreaseValue Action;
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
                if(valuePair.Action == ReduceIncreaseValue.Reduce)
                    _testHealthObject.Reduce(valuePair.Value);
                else if(valuePair.Action == ReduceIncreaseValue.Increase)
                    _testHealthObject.Gain(valuePair.Value);
            }
        }
    }
}
