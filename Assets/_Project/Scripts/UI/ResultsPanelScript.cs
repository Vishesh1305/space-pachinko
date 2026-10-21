using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using _Project.Scripts.Core;

namespace _Project.Scripts.UI
{
    public class ResultsPanel : MonoBehaviour
    {
        [Header("References")]
        [Tooltip("GameManager in Managers. Raises OnRoundEnded and provides Balls Used.")]
        [SerializeField] private GameManager gameManager;

        [Tooltip("ScoreManager in Managers. Provides the score breakdown.")]
        [SerializeField] private ScoreManager scoreManager;

        [Tooltip("Child object holding Dim and Panel. Hidden at start, shown when the round ends.")]
        [SerializeField] private GameObject root;

        [Header("Texts")]
        [Tooltip("Large final score text (FinalScore).")]
        [SerializeField] private TMP_Text finalScoreText;

        [Tooltip("SlotPoints text.")]
        [SerializeField] private TMP_Text slotPointsText;

        [Tooltip("PickupPoints text.")]
        [SerializeField] private TMP_Text pickupPointsText;

        [Tooltip("BallsUsed text.")]
        [SerializeField] private TMP_Text ballsUsedText;

        [Tooltip("JackpotHits text (200 slot hits).")]
        [SerializeField] private TMP_Text jackpotHitsText;

        [Header("Navigation")]
        [Tooltip("Button selected when the panel opens, so keyboard and gamepad work without a mouse (Replay).")]
        [SerializeField] private GameObject firstSelected;

        private void Awake()
        {
            if (root != null) root.SetActive(false);
            else Debug.LogError($"[Results] {name}: Root is not assigned. Drag the Root child into the field.", this);

            if (scoreManager == null)
            {
                Debug.LogError($"[Results] {name}: Score Manager is not assigned. Drag the ScoreManager object from Managers into the field.", this);
            }
        }

        private void OnEnable()
        {
            if (gameManager == null)
            {
                Debug.LogError($"[Results] {name}: Game Manager is not assigned. Drag the GameManager object from Managers into the field.", this);
                return;
            }

            gameManager.OnRoundEnded += HandleRoundEnded;
        }

        private void OnDisable()
        {
            if (gameManager != null)
            {
                gameManager.OnRoundEnded -= HandleRoundEnded;
            }
        }

        private void HandleRoundEnded()
        {
            if (scoreManager != null)
            {
                SetText(finalScoreText, scoreManager.TotalScore.ToString());
                SetText(slotPointsText, $"SLOT POINTS: {scoreManager.SlotPoints}");
                SetText(pickupPointsText, $"PICKUP POINTS: {scoreManager.PickupPoints}");
                SetText(jackpotHitsText, $"200 HITS: {scoreManager.JackpotHits}");
            }

            SetText(ballsUsedText, $"BALLS USED: {gameManager.BallsUsed}");

            if (root != null) root.SetActive(true);

            if (EventSystem.current != null && firstSelected != null)
            {
                EventSystem.current.SetSelectedGameObject(firstSelected);
            }
        }

        private static void SetText(TMP_Text target, string value)
        {
            if (target != null) target.text = value;
        }

        public void OnReplayClicked()
        {
            SceneLoader.LoadGame();
        }

        public void OnMainMenuClicked()
        {
            SceneLoader.LoadMainMenuScene();
        }
    }
}