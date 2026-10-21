using System;
using UnityEngine;
using UnityEngine.InputSystem;


namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(SpawnerMovement))]
    public class BallSpawner : MonoBehaviour
    {
        
        [Header("Input")]
        [Tooltip("Gameplay/Spawn action from GameControls.")]
        [SerializeField] private InputActionReference spawnAction = null;

        [Header("Spawning")]
        [Tooltip("Ball prefab from Prefabs/Ball.")]
        [SerializeField] private Rigidbody2D ballPrefab = null;
        
        [Tooltip("Balls root in the Game scene. Spawned balls are parented here.")]
        [SerializeField] private Transform ballContainer = null;

        [Tooltip("Minimum seconds between spawns. Uses scaled time, so pause freezes it.")]
        [Min(0f)]
        [SerializeField] private float spawnCooldown = 0.35f;
        
        [Tooltip("Random horizontal offset per spawn, in world units. Keep under 0.05.")]
        [Min(0f)]
        [SerializeField] private float spawnJitterX = 0.03f;

        [Header("Overlap Check")] 
        [Tooltip("Layers that block a spawn. Board and Obstacle only.")]
        [SerializeField] private LayerMask blockingLayers;
        
        [Tooltip("Ball world radius used for the overlap test. Match the Ball prefab.")]
        [SerializeField] private float ballRadius = 0.15f;
        
        [Tooltip("Extra margin added to the radius so a ball touching a collider is also refused.")]
        [SerializeField] private float overlapSkin = 0.01f;

        private float _nextSpawnTime = 0f;
        private SpawnerMovement _spawnerMovement;

        public static event Action OnBallSpawned;

        private void OnEnable()
        {
            if (spawnAction != null)
            {
                if (ballPrefab != null)
                {
                    if (ballContainer != null)
                    {
                        if (_spawnerMovement != null)
                        {
                            spawnAction.action.Enable();
                            return;
                        }
                    }
                }
            }
            Debug.LogError($"BallSpawner on {gameObject.name} Check the 4 serialized variables. Either one of them is not assigned. Disabling the component.", this);
            enabled = false;
        }

        private void OnDisable()
        {
            if (spawnAction != null) spawnAction.action.Disable();
        }

        private void Awake()
        {
            _spawnerMovement = GetComponent<SpawnerMovement>();
        }

        private void Update()
        {
            if (!spawnAction.action.WasPressedThisFrame()) return;
            if(Time.time < _nextSpawnTime) return;

            Vector3 spawnPosition = transform.position;
            spawnPosition.x += UnityEngine.Random.Range(-spawnJitterX, spawnJitterX);
            spawnPosition = _spawnerMovement.ClampToBounds(spawnPosition);

            Collider2D blocker = Physics2D.OverlapCircle(spawnPosition, ballRadius + overlapSkin, blockingLayers);
            if (blocker != null)
            {
                Debug.LogWarning($"BallSpawner on '{gameObject.name}': spawn refused at {spawnPosition}, overlapping '{blocker.gameObject.name}' on layer '{LayerMask.LayerToName(blocker.gameObject.layer)}'.", this);
                return;
            }

            Instantiate(ballPrefab, spawnPosition, Quaternion.identity, ballContainer);
            _nextSpawnTime = Time.time + spawnCooldown;
            
            OnBallSpawned?.Invoke();
        }
    }
}
