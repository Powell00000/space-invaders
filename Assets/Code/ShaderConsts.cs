using UnityEngine;

public static class ShaderConsts
{
    public static readonly string EmissionColor = "_EmissionColor";

    static int emissionId = -1;

    public static int EmissionID
    {
        get
        {
            if (emissionId == -1)
            {
                emissionId = Shader.PropertyToID(EmissionColor);
            }

            return emissionId;
        }
    }
}
