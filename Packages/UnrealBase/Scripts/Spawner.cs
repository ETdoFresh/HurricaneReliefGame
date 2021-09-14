using UnityEngine;

namespace UnrealBase
{
    public abstract class Spawner : ScriptableObject
    {
        public GameObject prefab;

        public static GameObject Spawn(Spawner spawner, Transform parent = null)
        {
            if (!spawner) return null;
            if (!spawner.prefab) return null;
            var newGameObject = Instantiate(spawner.prefab, parent);
            newGameObject.name = spawner.prefab.name;
            return newGameObject;
        }
    }
}