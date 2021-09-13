using System;
using UnityEngine;
using static CodeExtensions.ScriptableObject;

namespace UnrealBase
{
    [CreateAssetMenu(menuName = "Unreal Base/Project Settings", fileName = "ProjectSettings", order = 0)]
    public class ProjectSettings : ScriptableObject
    {
        [Header("Maps & Modes")] public GameModeBase defaultGameMode;

#if UNITY_EDITOR
        [Header("Default Maps")] public UnityEditor.SceneAsset gameDefaultMap;
        public UnityEditor.SceneAsset transitionMap;
        public UnityEditor.SceneAsset serverDefaultMap;
#endif
        [HideInInspector] public string gameDefaultMapName;
        [HideInInspector] public string transitionMapName;
        [HideInInspector] public string serverDefaultMapName;

        private void OnValidate()
        {
#if UNITY_EDITOR
            defaultGameMode = FindIfNull(defaultGameMode, "GameModeBase");
            gameDefaultMapName = gameDefaultMap ? gameDefaultMap.name : "";
            transitionMapName = transitionMap ? transitionMap.name : "";
            serverDefaultMapName = serverDefaultMap ? serverDefaultMap.name : "";
#endif
        }

        private void Reset()
        {
            OnValidate();
        }
    }
}