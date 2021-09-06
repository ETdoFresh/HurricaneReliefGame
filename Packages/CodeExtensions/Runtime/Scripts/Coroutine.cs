using System.Collections;
using UnityEngine;

namespace CodeExtensions
{
    public class Coroutine : MonoBehaviour
    {
        private static Coroutine _singleton;

        private static void CreateCoroutineGameObject()
        {
            if (_singleton) return;
            var coroutineGameObject = new GameObject("Coroutine");
            _singleton = coroutineGameObject.AddComponent<Coroutine>();
            DontDestroyOnLoad(coroutineGameObject);
        }

        // ReSharper disable once Unity.IncorrectMethodSignature
        public static UnityEngine.Coroutine Start(IEnumerator routine)
        {
            CreateCoroutineGameObject();
            return _singleton.StartCoroutine(routine);
        }

        public void Stop(UnityEngine.Coroutine coroutine)
        {
            CreateCoroutineGameObject();
            _singleton.StopCoroutine(coroutine);
        }
    }
}