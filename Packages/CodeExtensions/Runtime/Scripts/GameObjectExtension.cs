using UnityEngine;

namespace CodeExtensions
{
    public static class GameObjectExtension
    {
        public static GameObject Spawn(GameObject prefab)
        {
            var newGameObject = Object.Instantiate(prefab);
            newGameObject.name = prefab.name;
            return newGameObject;
        }

        public static T Spawn<T>(GameObject prefab) where T : Component
        {
            return Spawn(prefab).GetComponentInChildren<T>();
        }

        public static void CopyTransformTo(this GameObject source, GameObject target) =>
            source.transform.CopyTransformTo(target.transform);
        
        public static void CopyTransformFrom(this GameObject target, GameObject source) =>
            target.transform.CopyTransformFrom(source.transform);

        public static void CopyTransformTo(this GameObject source, Component target) =>
            source.transform.CopyTransformTo(target.transform);
        
        public static void CopyTransformFrom(this GameObject target, Component source) =>
            target.transform.CopyTransformFrom(source.transform);
    }
}