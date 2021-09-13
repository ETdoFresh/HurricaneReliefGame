using System;
using UnityEngine;
using static CodeExtensions.ScriptableObject;

namespace UnrealBase
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
            var pawn = defaultPawnClass.Spawn();
            var hud = hudClass.Spawn();
            var playerController = playerControllerClass.Spawn();
            var playerState = playerStateClass.Spawn();
            var gameState = gameStateClass.Spawn();
            var playerStart = FindObjectOfType<PlayerStart>();
            if (pawn && playerStart)
            {
                pawn.transform.position = playerStart.transform.position;
            }
        }

        protected virtual void Logout()
        {
        }

        public void StartGame()
        {
            InitGame();
            PreLogin();
            PostLogin();
            HandleStartingNewPlayer();
        }
    }
}