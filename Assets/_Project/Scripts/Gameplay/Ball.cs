using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class Ball : MonoBehaviour
    {
        [Header("Despawn")]
        [Tooltip("Speed (units/s) below which the ball counts as resting in a slot.")]
        [SerializeField, Min(0f)] private float settleSpeed = 0.05f;

        [Tooltip("Seconds the ball must stay below Settle Speed before it is destroyed.")]
        [SerializeField, Min(0f)] private float settleTime = 0.3f;

        [Tooltip("Failsafe: seconds after the claim when the ball is destroyed even if it never settles.")]
        [SerializeField, Min(0f)] private float maxDespawnTime = 3f;

        [Tooltip("Linear damping applied when the ball is claimed, so it brakes quickly in the slot.")]
        [SerializeField, Min(0f)] private float settleLinearDamping = 3f;

        [Tooltip("Angular damping applied when the ball is claimed, so it stops rolling.")]
        [SerializeField, Min(0f)] private float settleAngularDamping = 3f;

        private bool _isClaimed;
        public bool IsClaimed => _isClaimed;

        private Rigidbody2D _rigidbody;
        private bool _isDespawning;
        private float _settledTimer;
        private float _despawnTimer;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody2D>();
        }

        public bool TryClaim()
        {
            if (_isClaimed)
            {
                return false;
            }

            _isClaimed = true;
            return _isClaimed;
        }

        public void BeginDespawn()
        {
            if (_isDespawning) return;

            _isDespawning = true;
            _rigidbody.linearDamping = settleLinearDamping;
            _rigidbody.angularDamping = settleAngularDamping;
        }

        private void FixedUpdate()
        {
            if (!_isDespawning) return;

            float dt = Time.fixedDeltaTime;
            _despawnTimer += dt;

            if (_rigidbody.linearVelocity.sqrMagnitude < settleSpeed * settleSpeed)
            {
                _settledTimer += dt;
            }
            else
            {
                _settledTimer = 0f;
            }

            if (_settledTimer >= settleTime || _despawnTimer >= maxDespawnTime)
            {
                Destroy(gameObject);
            }
        }
    }
}