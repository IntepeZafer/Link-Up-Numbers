using UnityEngine;
using System;
using Game.Gameplay;

namespace Game.Core
{
    public class InputManager : MonoBehaviour
    {
        // Eventler: Başka sınıflar bu olayları dinleyecek (Temiz Kod: Decoupling)
        public static event Action<NumberNode> OnNodeSelected;
        public static event Action OnInputReleased;

        [SerializeField] private LayerMask nodeLayer; // Sadece sayıları algılamak için
        private Camera mainCamera;
        private bool isDragging = false;

        private void Awake()
        {
            mainCamera = Camera.main;
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            // Fare tıklandığında veya ekrana dokunulduğunda
            if (Input.GetMouseButtonDown(0))
            {
                isDragging = true;
                CheckForNode();
            }
            
            // Parmak ekranda sürüklenirken
            if (isDragging && Input.GetMouseButton(0))
            {
                CheckForNode();
            }

            // Parmak ekrandan çekildiğinde
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                OnInputReleased?.Invoke();
            }
        }

        private void CheckForNode()
        {
            // Ekrandaki dokunma noktasını dünya koordinatına çevir
            Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            
            // O noktada bir Collider var mı bak (Raycast)
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero, 0f, nodeLayer);

            if (hit.collider != null)
            {
                if (hit.collider.TryGetComponent<NumberNode>(out NumberNode node))
                {
                    // Bir sayıya dokunduk! Bunu ilgilenenlere haber ver.
                    OnNodeSelected?.Invoke(node);
                }
            }
        }
    }
}