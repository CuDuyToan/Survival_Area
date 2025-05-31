using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjBuildingPreview : MonoBehaviour
{
    private MeshRenderer _renderer;
    [SerializeField] private List<MeshRenderer> _meshes = new List<MeshRenderer>();

    [SerializeField] private LayerMask _ignoreLayer;

    [SerializeField] private Color _safeToBuild_Color;
    [SerializeField] private Color _unSafeToBuild_Color;

    private bool _isCanBuild = true;
    public bool _IsCanBuild
    {
        set
        {
            _isCanBuild = value;
        }
        get
        {
            return _isCanBuild;
        }
    }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
    }

    private bool IsInLayerMask(GameObject obj, LayerMask layerMask)
    {
        return ((1 << obj.layer) & layerMask) != 0;
    }

    private void OnTriggerStay(Collider other)
    {
        if (IsInLayerMask(other.gameObject, _ignoreLayer))
        {
            SetPreviewState(false);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (IsInLayerMask(other.gameObject, _ignoreLayer))
        {
            SetPreviewState(true);
        }
    }

    public void SetPreviewState(bool state)
    {
        _IsCanBuild = state;

        if (state == true) SetRendererColor(_safeToBuild_Color);
        else if (state == false) SetRendererColor(_unSafeToBuild_Color);
    }

    private void SetRendererColor(Color color)
    {
        if (_renderer != null)
        {
            _renderer.material.color = color;
        }else
        {
            foreach (Renderer mesh in _meshes)
            {
                mesh.material.color = color;
            }
        }
    }
}
