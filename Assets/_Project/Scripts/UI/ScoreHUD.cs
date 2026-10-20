using UnityEngine;
using TMPro;
using _Project.Scripts.Core;

namespace _Project.Scripts.UI
{
    public class ScoreHUD : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("Score Manager GameObject Reference")]
        [SerializeField] private ScoreManager scoreManager;
        
        [Tooltip("Score Text (TMP_Text type) reference")]
        [SerializeField] private TMP_Text scoreText;

        private void OnEnable()
        {
            if (scoreManager == null)
            {
                Debug.LogError($"[ScoreHUD] {name}: Score Manager is not assigned. Drag the ScoreManager object from Managers into the field.", this);
                return;
            }

            if (scoreText == null)
            {
                Debug.LogError($"[ScoreHUD] {name}: Score Text is not assigned. Drag Score_Value into the field.", this);
                return;
            }

            scoreManager.OnScoreChanged += HandleScoreChanged;
        }

        private void OnDisable()
        {
            if (scoreManager != null)
            {
                scoreManager.OnScoreChanged -= HandleScoreChanged;
            }
        }

        private void Start()
        {
            if (scoreManager == null || scoreText == null) return;
            HandleScoreChanged(scoreManager.TotalScore);
        }

        private void HandleScoreChanged(int text)
        {
            scoreText.text = text.ToString();
        }
    }
}
