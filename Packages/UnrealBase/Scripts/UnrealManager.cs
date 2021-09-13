using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static CodeExtensions.ScriptableObject;

namespace UnrealBase
{
    public class UnrealManager : MonoBehaviour
    {
        private static UnrealManager singleton;
        
        [SerializeField] private ProjectSettings projectSettings;

        [RuntimeInitializeOnLoadMethod]
        private static void CreateUnrealManagerSingleton()
        {
            var existingUnrealManager = FindObjectOfType<UnrealManager>();
            if (existingUnrealManager) return;
            
            var unrealManager = Resources.Load<GameObject>("UnrealManager");
            if (!unrealManager)
            {
                Debug.LogWarning("Could not find Resources/UnrealManager.prefab. Using DefaultUnrealManager. Please ensure you have a 'Resources' folder in your project which contains 'UnrealManager.prefab' containing the UnrealManager Component. Alternatively, you may also create a GameObject with UnrealManager Component in your scene.");
                unrealManager = Resources.Load<GameObject>("DefaultUnrealManager");
            }
            var newGameObject = Instantiate(unrealManager);
            newGameObject.name = "UnrealManager";
        }
        
        private void OnValidate()
        {
            projectSettings = FindIfNull(projectSettings, "ProjectSettings");
        }

        private void Awake()
        {
            if (!singleton)
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
                OnSceneLoaded(SceneManager.GetActiveScene(), LoadSceneMode.Single);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
                Destroy(gameObject);
        }

        private void OnDestroy()
        {
            if (singleton != this) return;
            SceneManager.sceneLoaded -= OnSceneLoaded;
            singleton = null;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode loadSceneMode)
        {
            if (loadSceneMode == LoadSceneMode.Additive) return;
            var worldSettingsReference = FindObjectOfType<WorldSettingsReference>();
            var gameMode = projectSettings.defaultGameMode;
            if (worldSettingsReference && worldSettingsReference.worldSettings && worldSettingsReference.worldSettings.gameModeOverride)
                gameMode = worldSettingsReference.worldSettings.gameModeOverride;
            gameMode.StartGame();
        }
    }
}
