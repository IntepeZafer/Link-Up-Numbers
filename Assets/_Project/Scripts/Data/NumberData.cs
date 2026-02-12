using UnityEngine;
using System.Collections.Generic;

namespace Game.Data
{
    [System.Serializable]
    public struct ValueColor
    {
        public int value;
        public Color color;
    }

    [CreateAssetMenu(fileName = "NewNumberData", menuName = "Game/Number Data")]
    public class NumberData : ScriptableObject
    {
        public List<ValueColor> fixedColors;

        public Color GetColorForValue(int value)
        {
            // 1. Listede tanımlı rengi ara
            foreach (var vc in fixedColors)
            {
                if (vc.value == value) return vc.color;
            }

            // 2. Eğer yoksa (çok büyük sayılar için) otomatik renk üret
            // Altın oran ve logaritma kullanarak birbirine yakın olmayan renkler üretir
            float hue = (Mathf.Log(value, 2) * 0.15f) % 1f;
            return Color.HSVToRGB(hue, 0.6f, 0.9f);
        }
    }
}