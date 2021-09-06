using System.Collections;
using UnityEngine;

namespace SimpleDialog.Utility
{
    public class Coroutine : MonoBehaviour
    {
        private static Coroutine singleton;

        private static void CreateCoroutineGameObject()
        {
            if (singleton) return;
            var coroutineGameObject = new GameObject("Coroutine");
            singleton = coroutineGameObject.AddComponent<Coroutine>();
            DontDestroyOnLoad(coroutineGameObject);
        }

        public static UnityEngine.Coroutine Start(IEnumerator routine)
        {
            CreateCoroutineGameObject();
            return singleton.StartCoroutine(routine);
        }

        public void Stop(UnityEngine.Coroutine coroutine)
        {
            CreateCoroutineGameObject();
            singleton.StopCoroutine(coroutine);
        }
    }
}