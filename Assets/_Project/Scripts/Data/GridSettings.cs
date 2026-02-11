using UnityEngine;

namespace Game.Data
{
    [CreateAssetMenu(fileName = "NewGridSettings" , menuName = "Game/Grid Settings")]
    public class GridSettings : ScriptableObject
    {
        public int width = 5; // Sütun Sayısı
        public int height = 5; // Satır Sayısı
        public float cellSize = 1.1f; // Hücreler Arası Mesafe
    }
}