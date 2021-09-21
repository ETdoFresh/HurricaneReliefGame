using UnityEngine;

namespace UnrealBlankTemplate
{
    [CreateAssetMenu(menuName = "Unreal Base/World Settings", fileName = "WorldSettings", order = 0)]
    public class WorldSettings : ScriptableObject
    {
        public GameModeBase gameModeOverride;
    }
}