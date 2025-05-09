using UnityEngine;

public class TilingMaterial : MonoBehaviour
{
    private Renderer _renderer => GetComponent<Renderer>();
    [SerializeField] private Material _instanceMaterial;
    [SerializeField] private Vector2 _tiling = new(1f, 1f);

    private void OnValidate()
    {
        // 인스펙터에서 값이 바뀌었을 때 실행됨
        ApplyTiling();
    }

    private void Awake()
    {
        ApplyTiling();
    }

    private void ApplyTiling()
    {
        if (_renderer != null && _renderer.sharedMaterial != null)
        {
            Material mat = new(_instanceMaterial);
            mat.mainTextureScale = _tiling;
            //mat.SetVector("_Tiling", _tiling);
            _renderer.material = mat;
        }
    }
}
