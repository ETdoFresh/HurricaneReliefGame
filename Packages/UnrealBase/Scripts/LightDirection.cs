using UnityEngine;

namespace UnrealBase
{
    public class LightDirection : MonoBehaviour
    {
        private static readonly int LightDirectionID = Shader.PropertyToID("_CustomSkyboxLightDirection");

        private void Update()
        {
            Shader.SetGlobalVector(LightDirectionID, transform.forward);
        }
    }
}
