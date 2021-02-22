using UnityEngine;

public static class RendererEx
{
    static MaterialPropertyBlock propertyBlock = new MaterialPropertyBlock();

    public static void SetEmission(this Renderer renderer, Color color)
    {
        renderer.GetPropertyBlock(propertyBlock);
        propertyBlock.SetColor(ShaderConsts.EmissionID, color);
        renderer.SetPropertyBlock(propertyBlock);
    }
}
