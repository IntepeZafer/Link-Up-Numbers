using UnityEngine;
using System;

namespace Game.Core
{
    public class ScoreManager : MonoBehaviour
    {
        public static ScoreManager Instance { get; private set; }

        // UI'ın dinleyeceği olaylar (Events)
        public static event Action<int> OnScoreChanged;
        public static event Action<int> OnHighScoreChanged;

        private int currentScore = 0;
        private int highScore = 0;

        private void Awake()
        {
            // Singleton Başlatma
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            // Kayıtlı yüksek skoru telefondan oku
            highScore = PlayerPrefs.GetInt("HighScore", 0);
        }

        private void Start()
        {
            // Oyun başlar başlamaz mevcut skorları UI'a bildir
            OnScoreChanged?.Invoke(currentScore);
            OnHighScoreChanged?.Invoke(highScore);
        }

        public void AddScore(int amount)
        {
            currentScore += amount;
            OnScoreChanged?.Invoke(currentScore);

            // Yüksek skor kontrolü
            if (currentScore > highScore)
            {
                highScore = currentScore;
                PlayerPrefs.SetInt("HighScore", highScore);
                OnHighScoreChanged?.Invoke(highScore);
            }
        }
    }
}