using System.Collections.Generic;
using UnityEngine;

namespace CodeExtensions
{
    public static class ObjectExtension
    {
        public static T Find<T>() where T : Object
        {
#if UNITY_EDITOR
            var t = typeof(T).Name;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{t}");
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
#else
            return null;
#endif
        }
        
        public static T Find<T>(string name) where T : Object
        {
#if UNITY_EDITOR
            var t = typeof(T).Name;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{t} {name}");
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
#else
            return null;
#endif
        }

        public static IEnumerable<T> FindAll<T>() where T : Object
        {
#if UNITY_EDITOR
            var t = typeof(T).Name;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{t}");
            foreach (var guid in guids)
            {
                var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guid);
                var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
                yield return asset;
            }
#else
            yield return null;
#endif
        }

        public static T FindIfNull<T>(T instance) where T : Object
        {
            return instance ? instance : Find<T>();
        }
        
        public static T FindIfNull<T>(T instance, string name) where T : Object
        {
            return instance ? instance : Find<T>(name);
        }
    }
}