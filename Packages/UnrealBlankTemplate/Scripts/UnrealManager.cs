using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static CodeExtensions.ObjectExtension;

namespace UnrealBlankTemplate
{
    public class UnrealManager : MonoBehaviour
    {
        private static UnrealManager singleton;
        
        [SerializeField] private ProjectSettings projectSettings;
        [SerializeField] private GameObject playerCameraManagerPrefab;
        [SerializeField] private InputActionAsset inputActionAsset;

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
            playerCameraManagerPrefab = FindIfNull(playerCameraManagerPrefab, "PlayerCameraManager");
        }

        private void Awake()
        {
            if (!singleton)
            {
                singleton = this;
                DontDestroyOnLoad(gameObject);
                SceneManager.sceneLoaded += OnSceneLoaded;
            }
            else
                Destroy(gameObject);
        }

        private void OnEnable()
        {
            if (inputActionAsset) inputActionAsset.Enable();
        }

        private void OnDisable()
        {
            if (inputActionAsset) inputActionAsset.Disable();
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
            gameMode.StartGame(playerCameraManagerPrefab);
        }
    }
}
