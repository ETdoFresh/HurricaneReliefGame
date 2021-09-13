using UnityEngine;

namespace UnrealBase
{
    public abstract class Spawner : ScriptableObject
    {
        public GameObject prefab;

        public GameObject Spawn(Transform parent = null)
        {
            if (!prefab) return null;
            var newGameObject = Instantiate(prefab, parent);
            newGameObject.name = prefab.name;
            return newGameObject;
        }
    }
}