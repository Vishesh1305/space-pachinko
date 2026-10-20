using System;
using UnityEngine;
using _Project.Scripts.Gameplay;

namespace _Project.Scripts.Core
{
    public class ScoreManager : MonoBehaviour
    {
        [Header("Debug")]
        [Tooltip("Logs every score change to the console.")]
        [SerializeField] private bool logScoreChanges = true;

        private int _slotPoints = 0;
        private int _pickupPoints = 0;
        private int _jackpotHits = 0;
        private int _ballsScored = 0;
        
        public int SlotPoints => _slotPoints;
        public int PickupPoints => _pickupPoints;
        public int JackpotHits => _jackpotHits;
        public int BallsScored => _ballsScored;
        public int TotalScore => _slotPoints + _pickupPoints;

        public event Action<int> OnScoreChanged;

        private void OnEnable()
        {
            Slot.OnBallScored += HandleBallScored;
            Pickup.OnPickupCollected += HandlePickupCollected;
        }

        private void OnDisable()
        {
            Slot.OnBallScored -= HandleBallScored;
            Pickup.OnPickupCollected -= HandlePickupCollected;
        }

        private void HandleBallScored(Slot slot)
        {
            _slotPoints += slot.Points;
            _ballsScored += 1;

            if (slot.IsJackpot) _jackpotHits += 1;
            
            OnScoreChanged?.Invoke(TotalScore);
            
            if(logScoreChanges) Debug.Log($"[Score] {slot.name} +{slot.Points}, total {TotalScore}", this);
        }

        private void HandlePickupCollected(Pickup p)
        {
            _pickupPoints += p.Points;
            OnScoreChanged?.Invoke(TotalScore);
            if(logScoreChanges) Debug.Log($"[Score] {p.name} +{p.Points}, total {TotalScore}", this);
        }
    }
}
