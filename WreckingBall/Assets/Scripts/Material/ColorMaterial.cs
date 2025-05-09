using UnityEngine;

[ExecuteAlways] // 에디터에서도 반영되게
public class ColorMaterial : MonoBehaviour
{
    [SerializeField] private Color color = Color.white;
    //[Range(0f, 1f)][SerializeField] private float metallic = 1f;
    //[Range(0f, 1f)][SerializeField] private float smoothness = 0.9f;

    private Renderer _renderer => GetComponent<Renderer>();
    [SerializeField] private Material _instanceMaterial;


    private void OnValidate()
    {
        // 인스펙터에서 값이 바뀌었을 때 실행됨
        ApplyColor();
    }

    private void Awake()
    {
        ApplyColor();
    }

    private void ApplyColor()
    {
        if (_renderer != null && _renderer.sharedMaterial != null)
        {
            Material mat = new(_instanceMaterial);
            mat.color = color;
            _renderer.material = mat;
            //_instanceMaterial.SetFloat("_Metallic", metallic);
            //_instanceMaterial.SetFloat("_Smoothness", smoothness);
        }
    }
}
