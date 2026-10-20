using System;
using TMPro;
using UnityEngine;

namespace _Project.Scripts.Gameplay
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class Slot : MonoBehaviour
    {
        [Header("Scoring")]
        [Tooltip("Points awarded when a ball lands in this slot.")]
        [SerializeField, Min(0)] private int points = 10;

        [Tooltip("Counts toward 200 slot hits on the Results screen.")]
        [SerializeField] private bool isJackpot;

        [Header("References")]
        [Tooltip("Optional world text that shows the point value.")]
        [SerializeField] private TMP_Text label;

        private Collider2D _collider;

        public int Points => points;
        public bool IsJackpot => isJackpot;

        public static event Action<Slot> OnBallScored;

        private void Awake()
        {
            _collider = GetComponent<Collider2D>();

            if (!_collider.isTrigger)
            {
                Debug.LogWarning($"{name}: Collider2D is not a trigger. Balls will bounce off this slot instead of scoring.", this);
            }

            if (label != null)
            {
                label.text = points.ToString();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.attachedRigidbody) return;

            if (!other.attachedRigidbody.TryGetComponent(out Ball ball))
            {
                Debug.LogWarning($"{name}: {other.attachedRigidbody.name} entered the slot but has no Ball component.", this);
                return;
            }

            if (!ball.TryClaim()) return;

            OnBallScored?.Invoke(this);

            //Destroy(other.attachedRigidbody.gameObject); 
            ball.BeginDespawn();
        }
    }
}