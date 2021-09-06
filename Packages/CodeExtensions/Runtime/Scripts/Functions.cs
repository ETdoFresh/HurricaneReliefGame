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
    }
}