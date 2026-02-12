using UnityEngine;
using TMPro;

namespace Game.Gameplay
{
    public class NumberNode : MonoBehaviour
    {
        [SerializeField] private TMP_Text textDisplay;
        [SerializeField] private SpriteRenderer spriteRenderer;
        
        public int Value { get; private set; }
        public int GridX { get; private set; }
        public int GridY { get; private set; }

        public void SetValue(int value, Color color)
        {
            Value = value;
            if (textDisplay != null) textDisplay.text = value.ToString();
            if (spriteRenderer != null) spriteRenderer.color = color;
        }

        public void SetGridPosition(int x, int y)
        {
            GridX = x;
            GridY = y;
        }

        // --- YENİ: Pürüzsüz Hareket Metodu ---
        public void MoveToPosition(Vector3 targetPos)
        {
            StopCoroutine("MoveCoroutine");
            StartCoroutine(MoveCoroutine(targetPos));
        }

        private System.Collections.IEnumerator MoveCoroutine(Vector3 targetPos)
        {
            float duration = 0.2f; // Düşüş hızı
            float elapsed = 0;
            Vector3 startPos = transform.position;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.position = targetPos;
        }

        public void PlayPopEffect()
        {
            StopAllCoroutines();
            StartCoroutine(PopCoroutine());
        }

        private System.Collections.IEnumerator PopCoroutine()
        {
            Vector3 originalScale = new Vector3(0.7f, 0.7f, 1f);
            Vector3 targetScale = originalScale * 1.2f;
            float duration = 0.1f;
            float elapsed = 0;

            while (elapsed < duration)
            {
                transform.localScale = Vector3.Lerp(originalScale, targetScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            elapsed = 0;
            while (elapsed < duration)
            {
                transform.localScale = Vector3.Lerp(targetScale, originalScale, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            transform.localScale = originalScale;
        }

        public void MergeInto(Vector3 targetPos, System.Action onComplete)
        {
            StartCoroutine(MergeCoroutine(targetPos, onComplete));
        }

        private System.Collections.IEnumerator MergeCoroutine(Vector3 targetPos, System.Action onComplete)
        {
            float duration = 0.15f;
            float elapsed = 0;
            Vector3 startPos = transform.position;
            Vector3 startScale = transform.localScale;

            while (elapsed < duration)
            {
                transform.position = Vector3.Lerp(startPos, targetPos, elapsed / duration);
                transform.localScale = Vector3.Lerp(startScale, Vector3.zero, elapsed / duration);
                elapsed += Time.deltaTime;
                yield return null;
            }
            onComplete?.Invoke();
            Destroy(gameObject);
        }
    }
}