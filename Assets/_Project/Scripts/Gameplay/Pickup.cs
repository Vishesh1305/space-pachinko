using System;
using System.Collections;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(CircleCollider2D))]
    public class Pickup : MonoBehaviour
    {
        [Header("Scoring")]
        [Tooltip("Points awarded when a ball collects this pickup.")]
        [SerializeField, Min(0)] private int points = 5;

        [Header("Respawn")]
        [Tooltip("Seconds (scaled time) before the pickup reappears after being collected.")]
        [SerializeField, Min(0f)] private float respawnDelay = 8f;

        [Header("References")]
        [Tooltip("Optional. Found automatically on this object or its children if left empty.")]
        [SerializeField] private SpriteRenderer spriteRenderer;

        private Collider2D _collider;
        private bool _isAvailable = true;
        private Coroutine _respawnRoutine;

        public int Points => points;

        public static event Action<Pickup> OnPickupCollected;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            if (!_collider.isTrigger)
            {
                Debug.LogWarning($"{name}: Collider2D is not a trigger. Balls will bounce off this pickup instead of collecting it.", this);
            }

            if (spriteRenderer == null)
            {
                spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            }

            if (spriteRenderer == null)
            {
                Debug.LogWarning($"{name}: No SpriteRenderer found. The pickup will not hide when collected.", this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!_isAvailable) return;
            if (!other.attachedRigidbody) return;

            if (!other.attachedRigidbody.TryGetComponent(out Ball ball))
            {
                Debug.LogWarning($"{name}: {other.attachedRigidbody.name} entered the pickup but has no Ball component.", this);
                return;
            }

            if (ball.IsClaimed) return;

            _isAvailable = false;
            SetVisible(false);

            OnPickupCollected?.Invoke(this);

            _respawnRoutine = StartCoroutine(RespawnAfterDelay());
        }

        private IEnumerator RespawnAfterDelay()
        {
            yield return new WaitForSeconds(respawnDelay);

            SetVisible(true);
            _isAvailable = true;
            _respawnRoutine = null;
        }

        private void SetVisible(bool visible)
        {
            if (spriteRenderer != null) spriteRenderer.enabled = visible;
            _collider.enabled = visible;
        }

        private void OnDisable()
        {
            if (_respawnRoutine != null)
            {
                StopCoroutine(_respawnRoutine);
                _respawnRoutine = null;
            }

            SetVisible(true);
            _isAvailable = true;
        }
    }
}