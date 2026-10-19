using System;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PaddleRotator : MonoBehaviour
    {
        [Tooltip("Degrees per second. Positive spins counterclockwise, negative clockwise")]
        [SerializeField] private float angularSpeed = 90f;

        private Rigidbody2D _rigidbody;

        private void Awake()
        {
            _rigidbody = gameObject.GetComponent<Rigidbody2D>();
            if (_rigidbody.bodyType != RigidbodyType2D.Kinematic)
            {
                Debug.LogWarning($"HorizontalMover on {gameObject.name}: The attached rigidbody2D component on this game object is not Kinematic. It must be Kinematic.");
            }
        }

        private void FixedUpdate()
        {
            float targetRotation = _rigidbody.rotation + angularSpeed * Time.fixedDeltaTime;
            _rigidbody.MoveRotation(targetRotation);
        }

        private void OnDrawGizmos()
        {
            BoxCollider2D child = GetComponentInChildren<BoxCollider2D>();
            if (child == null) return;

            float childWorldWidth = child.size.x * child.transform.lossyScale.x;
            float childWorldHeight = child.size.y * child.transform.lossyScale.y;

            Vector2 dimensions = new Vector2(childWorldWidth, childWorldHeight);
            float sweepRadius = dimensions.magnitude / 2;
            
            Gizmos.color = Color.steelBlue;
            Gizmos.DrawWireSphere(transform.position, sweepRadius);
        }
    }
}
