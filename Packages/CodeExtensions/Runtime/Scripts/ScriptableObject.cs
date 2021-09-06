using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace CodeExtensions
{
    public static class ScriptableObject
    {
        public static T Find<T>() where T : UnityEngine.ScriptableObject
        {
#if UNITY_EDITOR
            var t = typeof(T).Name;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{t}");
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
#endif
        }
        
        public static T Find<T>(string name) where T : UnityEngine.ScriptableObject
        {
#if UNITY_EDITOR
            var t = typeof(T).Name;
            var guids = UnityEditor.AssetDatabase.FindAssets($"t:{t} {name}");
            var assetPath = UnityEditor.AssetDatabase.GUIDToAssetPath(guids[0]);
            var asset = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(assetPath);
            return asset;
#endif
        }

        public static IEnumerable<T> FindAll<T>() where T : UnityEngine.ScriptableObject
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
#endif
        }

        public static T FindIfNull<T>(T instance) where T : UnityEngine.ScriptableObject
        {
            return instance ? instance : Find<T>();
        }

#if UNITY_EDITOR
        [UnityEditor.MenuItem("Assets/Create/Asset from ScriptableObject", true)]
        private static bool CreateScriptableObjAsAssetValidator()
        {
            var activeObject = UnityEditor.Selection.activeObject;

            // make sure it is a text asset
            if ((activeObject == null) || !(activeObject is TextAsset))
            {
                return false;
            }

            // make sure it is a persistant asset
            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(activeObject);
            if (assetPath == null)
            {
                return false;
            }

            // load the asset as a monoScript
            var monoScript = (UnityEditor.MonoScript)UnityEditor.AssetDatabase.LoadAssetAtPath(assetPath, typeof(UnityEditor.MonoScript));
            if (monoScript == null)
            {
                return false;
            }

            // get the type and make sure it is a scriptable object
            var scriptType = monoScript.GetClass();
            if (scriptType == null || !scriptType.IsSubclassOf(typeof(UnityEngine.ScriptableObject)))
            {
                return false;
            }

            return true;
        }

        [UnityEditor.MenuItem("Assets/Create/Asset from ScriptableObject")]
        private static void CreateScriptableObjectAssetMenuCommand(UnityEditor.MenuCommand command)
        {
            // we already validated this path, and know these calls are safe
            var activeObject = UnityEditor.Selection.activeObject;
            var assetPath = UnityEditor.AssetDatabase.GetAssetPath(activeObject);
            var script = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEditor.MonoScript>(assetPath);
            CreateNewInstance(assetPath, script);
        }

        private static void CreateNewInstance(string assetPath, UnityEditor.MonoScript script)
        {
            var scriptType = script.GetClass();
            var path = Path.Combine(Path.GetDirectoryName(assetPath) ?? string.Empty, scriptType.Name + ".asset");
            try
            {
                var inst = UnityEngine.ScriptableObject.CreateInstance(scriptType);
                if (UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path))
                    path = UnityEditor.AssetDatabase.GenerateUniqueAssetPath(path);
                UnityEditor.AssetDatabase.CreateAsset(inst, path);
            }
            catch (Exception e)
            {
                Debug.LogException(e);
            }
        }
    }
#endif
}