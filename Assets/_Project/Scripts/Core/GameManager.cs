using System;
using UnityEngine;
using _Project.Scripts.Gameplay;

namespace _Project.Scripts.Core
{
    public class GameManager : MonoBehaviour
    {
        [Header("Round")]
        [Min(1), SerializeField] private int ballsPerRound = 20;
        
        [Header("References")]
        [SerializeField] private BallSpawner ballSpawner;
        
        [SerializeField] private SpawnerMovement spawnerMovement;

        [SerializeField] private Transform ballContainer;
        
        [Header("Debug")]
        [SerializeField] private bool logStateChanges = true;

        private int _ballsRemaining;
        private GameState _state;

        public GameState State => _state;
        public int BallsRemaining => _ballsRemaining;
        public int BallsUsed => ballsPerRound - _ballsRemaining;

        public event Action<int> OnBallsRemainingChanged;
        public event Action OnRoundEnded;

        private void Awake()
        {
            if(!ballSpawner) Debug.LogError($"[Game] {name}: Ball Spawner is not assigned. Drag the Spawner object into the field.", this);
            if(!spawnerMovement) Debug.LogError($"[Game] {name}: Spawner Movement is not assigned. Drag the Spawner object into the field.", this);
            if(!ballContainer) Debug.LogError($"[Game] {name}: Ball Container is not assigned. Drag the Balls object into the field.", this);

            _ballsRemaining = ballsPerRound;
            _state = GameState.Playing;
        }

        private void OnEnable()
        {
            BallSpawner.OnBallSpawned += HandleBallSpawned;
        }

        private void OnDisable()
        {
            BallSpawner.OnBallSpawned -= HandleBallSpawned;
        }

        private void HandleBallSpawned()
        {
            if (_state != GameState.Playing) return;
            _ballsRemaining -= 1;
            OnBallsRemainingChanged?.Invoke(_ballsRemaining);
            
            if(logStateChanges) Debug.Log($"[Game] Ball spawned, {_ballsRemaining} left", this);

            if (_ballsRemaining == 0 && ballSpawner != null)
            {
                ballSpawner.enabled = false;
            }
        }

        private void Update()
        {
            if (!ballContainer) return;
            if (_state == GameState.Playing && _ballsRemaining == 0 && ballContainer.childCount == 0)
            {
                EndRound();
            }
        }

        private void EndRound()
        {
            _state = GameState.Roundover;
            if (logStateChanges) Debug.Log("[Game] Round over", this);
            if (ballSpawner) ballSpawner.enabled = false;
            if (spawnerMovement) spawnerMovement.enabled = false;
            OnRoundEnded?.Invoke();
        }
    }
}