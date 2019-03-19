using UnityEngine;

[ExecuteInEditMode]
public class PostProcessingEffect : MonoBehaviour
{
    [SerializeField] private Material effect;

    private void Awake()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.DepthNormals;
    }

    private void OnRenderImage(RenderTexture a, RenderTexture b)
    {
        Graphics.Blit(a, b, effect);
    }
}
