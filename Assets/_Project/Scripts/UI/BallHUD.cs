using UnityEngine;
using TMPro;
using _Project.Scripts.Core;

namespace _Project.Scripts.UI
{
    public class BallsHUD : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("GameManager in Managers. Source of the balls remaining count.")]
        [SerializeField] private GameManager gameManager;

        [Tooltip("Text that shows the balls remaining (Balls_Value).")]
        [SerializeField] private TMP_Text ballsText;

        private void OnEnable()
        {
            if (gameManager == null)
            {
                Debug.LogError($"[BallsHUD] {name}: Game Manager is not assigned. Drag the GameManager object from Managers into the field.", this);
                return;
            }

            if (ballsText == null)
            {
                Debug.LogError($"[BallsHUD] {name}: Balls Text is not assigned. Drag Balls_Value into the field.", this);
                return;
            }

            gameManager.OnBallsRemainingChanged += HandleBallsRemainingChanged;
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnBallsRemainingChanged -= HandleBallsRemainingChanged;
            }
        }

        private void Start()
        {
            if (gameManager == null || ballsText == null) return;

            HandleBallsRemainingChanged(gameManager.BallsRemaining);
        }

        private void HandleBallsRemainingChanged(int ballsRemaining)
        {
            ballsText.text = ballsRemaining.ToString();
        }
    }
}