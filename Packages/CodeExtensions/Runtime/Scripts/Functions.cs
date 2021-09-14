using UnityEngine;

namespace CodeExtensions
{
    public static class Functions
    {
        public static bool IsNull(object obj) => obj.Equals(null);

        public static void Pause()
        {
            Time.timeScale = 0;
            AudioListener.pause = true;
        }

        public static void Resume()
        {
            Time.timeScale = 1;
            AudioListener.pause = false;
        }

        public static GameObject Spawn(GameObject prefab)
        {
            var newGameObject = Object.Instantiate(prefab);
            newGameObject.name = prefab.name;
            return newGameObject;
        }

        public static T Spawn<T>(GameObject prefab) where T : Component
        {
            var newGameObject = Object.Instantiate(prefab);
            newGameObject.name = prefab.name;
            return newGameObject.GetComponentInChildren<T>();
        }
    }
}