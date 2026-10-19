using System;
using Unity.Mathematics;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class HorizontalMover : MonoBehaviour
    {
        [Tooltip("Half the total travel, world units")]
        [Min(0)]
        [SerializeField] private float amplitude = 1.0f;
        
        [Tooltip("Seconds for one full left, right, left cycle")]
        [SerializeField] private float period = 4f;
        
        [Tooltip("Where in the cycle it starts, as a fraction of a cycle")]
        [Range(0,1)]
        [SerializeField] private float phaseOffset;

        private Rigidbody2D _rigidbody;
        private Vector2 _center;
        private float _elapsed;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody.bodyType != RigidbodyType2D.Kinematic)
            {
                Debug.LogWarning($"HorizontalMover on {gameObject.name}: The attached rigidbody2D component on this game object is not Kinematic. It must be Kinematic.");
            }

            _center = _rigidbody.position;
            _elapsed = 0f;

            if (period > 0)
            {
                float startX = _center.x + amplitude * Mathf.Sin(2 * Mathf.PI * phaseOffset);
                _rigidbody.position = new Vector2(startX, _center.y);
            }
        }

        private void FixedUpdate()
        {
            if (period <= 0) return;
            _elapsed += Time.fixedDeltaTime;

            float targetAngle = 2 * Mathf.PI * (_elapsed / period + phaseOffset);
            float targetX = _center.x + amplitude * Mathf.Sin(targetAngle);
            _rigidbody.MovePosition(new Vector2(targetX, _center.y));
        }

        private void OnValidate()
        {
            if (period <= 0)
            {
                Debug.LogWarning($"HorizontalMover on {gameObject.name}: The value of the variable 'period' must be above 0. Current Value:{period}.");
            }
        }

        private void OnDrawGizmos()
        {
            Vector2 center = Application.isPlaying ? _center : (Vector2)this.transform.position;
            Gizmos.color = Color.yellow;
            
            Gizmos.DrawLine(new (center.x - amplitude, center.y, 0), new(center.x + amplitude, center.y,0));
            
            Collider2D child = GetComponentInChildren<Collider2D>();
            if (child == null) return;
            
            Bounds worldBounds = child.bounds;
            if(worldBounds.size == Vector3.zero) return;

            float shiftL = (center.x - amplitude) - transform.position.x;
            Gizmos.DrawWireCube(worldBounds.center + new Vector3(shiftL, 0,0), worldBounds.size);

            float shiftR = (center.x + amplitude) - transform.position.x;
            Gizmos.DrawWireCube(worldBounds.center + new Vector3(shiftR, 0,0), worldBounds.size);

        }
    }
}
