using System.Collections.Generic;
using System.Linq;
using Mono.Cecil;
using UnityEditor.SpeedTree.Importer;
using UnityEngine;

public class IFrameFlash : MonoBehaviour
{
    [SerializeField] private Material _flashTexture;
    [SerializeField] private Health _health;
    [SerializeField] private GameObject _parentOfMeshRenderers;

    private SkinnedMeshRenderer[] _renderers;
    private bool _isFlashActive = false;
    private struct MeshRendererMaterialPairs
    {
        public SkinnedMeshRenderer MeshRenderer;
        public Material Material;
    }

    private Dictionary<GameObject, MeshRendererMaterialPairs> childCollection = new Dictionary<GameObject, MeshRendererMaterialPairs>();

    public void AddFlashMaterialToAllRenderers()
    {
        if (_isFlashActive)
            return;

        Debug.Log($"[IFRAME FLASH] Activating IFrame flash on object: {gameObject.name} -");

        foreach (var obj in childCollection)
        {
            var value = obj.Value;
            List<Material> currMaterials = value.MeshRenderer.materials.ToList();
            currMaterials.Add(_flashTexture);
            obj.Value.MeshRenderer.materials = currMaterials.ToArray();
        }

        _isFlashActive = true;
    }

    public void RemoveFlashMaterialFromAllRenderers()
    {
        if (!_isFlashActive)
            return;
    
        Debug.Log($"[IFRAME FLASH] Removing IFrame flash from object: {gameObject.name} -");

        foreach (var obj in childCollection)
        {
            var value = obj.Value;
            List<Material> currMaterials = value.MeshRenderer.materials.ToList();
            currMaterials.RemoveAll(mat => mat.name.StartsWith(_flashTexture.name));
            obj.Value.MeshRenderer.materials = currMaterials.ToArray();
        }

        _isFlashActive = false;
    }

    private void Awake()
    {
        _renderers = GetComponentsInChildren<SkinnedMeshRenderer>();

        foreach (var renderer in _renderers)
        {
            childCollection.Add(
                renderer.gameObject,
                new MeshRendererMaterialPairs
                {
                    MeshRenderer = renderer,
                    Material = renderer.material
                }
            );
        }

        _health = GetComponent<Health>();

        _health.OnIFrameStart.AddListener(AddFlashMaterialToAllRenderers);
        _health.OnIFrameEnd.AddListener(RemoveFlashMaterialFromAllRenderers);

    }
}