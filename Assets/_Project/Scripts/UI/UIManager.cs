using UnityEngine;
using TMPro;
using Game.Core;
using UnityEngine.SceneManagement;
namespace Game.UI
{
    public class UIManager : MonoBehaviour
    {
        [Header("UI References")]
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text highScoreText;
        [SerializeField] private GameObject gameOverPanel;

        private void OnEnable()
        {
            // ScoreManager'dan gelen haberleri dinlemeye başla
            ScoreManager.OnScoreChanged += UpdateScoreUI;
            ScoreManager.OnHighScoreChanged += UpdateHighScoreUI;
        }

        private void OnDisable()
        {
            // Abonelikten çık (Bellek sızıntısını önlemek için)
            ScoreManager.OnScoreChanged -= UpdateScoreUI;
            ScoreManager.OnHighScoreChanged -= UpdateHighScoreUI;
        }

        private void UpdateScoreUI(int score)
        {
            if (scoreText != null)
                scoreText.text = "SCORE: " + score.ToString();
        }

        private void UpdateHighScoreUI(int highScore)
        {
            if (highScoreText != null)
                highScoreText.text = "BEST: " + highScore.ToString();
        }
        public void ShowGameOver()
        {
            gameOverPanel.SetActive(true);
        }
        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}