using System.Linq;
using Cinemachine;
using CodeExtensions;
using UnityEngine;
using static CodeExtensions.GameObjectExtension;
using static CodeExtensions.ObjectExtension;
using static UnrealBlankTemplate.Spawner;

namespace UnrealBlankTemplate
{
    [CreateAssetMenu(menuName = "Unreal Base/Game Mode Base", fileName = "GameModeBase", order = 0)]
    public class GameModeBase : ScriptableObject
    {
        [Header("Game Mode")] public bool useSeamlessTravel;
        public bool pauseable = true;
        public bool startPlayersAsSpectators;
        public bool delayedStart;
        public float minimumResponseDelay = 1.0f;
        public string defaultPlayerName = "";
        public float inactivePlayerStateLifeSpan = 300.0f;

        [Header("Tick")] public bool startWithTickEnabled = true;

        [Header("Classes")] public PawnSpawner defaultPawnClass;
        public HUDSpawner hudClass;
        public PlayerControllerSpawner playerControllerClass;
        public SpectatorSpawner spectatorClass;
        public PlayerStateSpawner playerStateClass;
        public GameStateSpawner gameStateClass;
        private GameObject _playerCameraManagerPrefab;
        private CinemachineVirtualCamera _playerCameraManager;

        private void OnValidate()
        {
            defaultPawnClass = FindIfNull(defaultPawnClass, "DefaultPawn");
            hudClass = FindIfNull(hudClass, "HUD");
            playerControllerClass = FindIfNull(playerControllerClass, "PlayerController");
            spectatorClass = FindIfNull(spectatorClass, "SpectatorPawn");
            playerStateClass = FindIfNull(playerStateClass, "PlayerState");
            gameStateClass = FindIfNull(gameStateClass, "GameStateBase");
        }

        private void Reset()
        {
            OnValidate();
        }

        // Created from https://docs.unrealengine.com/4.27/en-US/InteractiveExperiences/Framework/GameMode/

        protected virtual void InitGame()
        {
            _playerCameraManager = Spawn<CinemachineVirtualCamera>(_playerCameraManagerPrefab);
        }

        protected virtual void PreLogin()
        {
        }

        protected virtual void PostLogin()
        {
        }

        protected virtual void HandleStartingNewPlayer()
        {
            SpawnDefaultPawnAtTransform();
        }

        protected virtual void RestartPlayer()
        {
        }

        protected virtual void RestartPlayerAtPlayerStart()
        {
        }

        protected virtual void RestartPlayerAtTransform()
        {
        }

        protected virtual void SpawnDefaultPawnAtTransform()
        {
            var gameState = Spawn(gameStateClass);
            var hud = Spawn(hudClass);

            var playerController = Spawn<PlayerController>(playerControllerClass);
            var playerState = Spawn(playerStateClass);
            playerController.AssignPlayerCameraManager(_playerCameraManager);
            
            var pawn = FindObjectsOfType<Pawn>()
                .FirstOrDefault(x => (int)x.autoPossessPlayer == playerController.MyPlayerIndex);
            
            if (!pawn)
            {
                pawn = Spawn<Pawn>(defaultPawnClass);
                var playerStart = FindObjectOfType<PlayerStart>();
                if (pawn && playerStart)
                    pawn.CopyTransformFrom(playerStart);
            }

            if (playerController && pawn)
                playerController.Possess(pawn);
        }

        protected virtual void Logout()
        {
        }

        public void StartGame(GameObject playerCameraManagerPrefab)
        {
            _playerCameraManagerPrefab = playerCameraManagerPrefab;
            InitGame();
            PreLogin();
            PostLogin();
            HandleStartingNewPlayer();
        }
    }
}