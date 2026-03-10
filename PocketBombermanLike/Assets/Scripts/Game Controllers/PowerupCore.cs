using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class PowerupCore : MonoBehaviour
{

    [SerializeField] private List<StatModifierMaterialPair> _pairs = new();
    
    private List<DecalProjector> _projectors = new();
    private StatModifier _statModifier;

    private void Awake()
    {
        _statModifier = GetComponent<StatModifier>();
        
        foreach (var projector in GetComponentsInChildren<DecalProjector>())
        {
            _projectors.Add(projector);
            Debug.Log($"[POWERUP CORE] Adding 1 decal projector to collection... Total: {_projectors.Count} -");
        }
    }

    /// <summary>
    /// Initializes the visual representation by matching the <see cref="StatModifier"/> type 
    /// with the corresponding material from <see cref="_pairs"/> and applying it to all <see cref="DecalProjector"/> components.
    /// </summary>
    private void Start()
    {
        Debug.Log($"[POWERUP CORE] Setting decal texture for stat type: {_statModifier.GetType} -");
        
        foreach (var projector in _projectors)
        {
            projector.material = _pairs.First(pair => pair.StatType == _statModifier.GetType).Material;
        }
    }

}