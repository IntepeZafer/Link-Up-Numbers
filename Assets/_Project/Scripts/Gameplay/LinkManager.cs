using UnityEngine;
using System.Collections.Generic;
using Game.Core;
using Game.Data;
using Game.UI;

namespace Game.Gameplay
{
    public class LinkManager : MonoBehaviour
    {
        [SerializeField] private LineRenderer lineRenderer;
        [SerializeField] private GridManager gridManager;
        [SerializeField] private NumberData numberData;

        private List<NumberNode> selectedNodes = new List<NumberNode>();
        private int currentLinkValue = -1;

        private void OnEnable() { InputManager.OnNodeSelected += HandleNodeSelected; InputManager.OnInputReleased += HandleInputReleased; }
        private void OnDisable() { InputManager.OnNodeSelected -= HandleNodeSelected; InputManager.OnInputReleased -= HandleInputReleased; }

        private void HandleNodeSelected(NumberNode node)
        {
            if (selectedNodes.Count == 0) { AddNode(node); return; }
            if (selectedNodes[selectedNodes.Count - 1] == node) return;
            if (selectedNodes.Count > 1 && selectedNodes[selectedNodes.Count - 2] == node) { RemoveLastNode(); return; }

            NumberNode lastNode = selectedNodes[selectedNodes.Count - 1];
            int diffX = Mathf.Abs(node.GridX - lastNode.GridX);
            int diffY = Mathf.Abs(node.GridY - lastNode.GridY);
            if (node.Value == currentLinkValue && !selectedNodes.Contains(node) && (diffX + diffY == 1)) AddNode(node);
        }

        private void AddNode(NumberNode node) { selectedNodes.Add(node); currentLinkValue = node.Value; node.PlayPopEffect(); UpdateLine(); }
        private void RemoveLastNode() { if (selectedNodes.Count > 0) { selectedNodes.RemoveAt(selectedNodes.Count - 1); UpdateLine(); } }

        private void UpdateLine()
        {
            lineRenderer.positionCount = selectedNodes.Count;
            for (int i = 0; i < selectedNodes.Count; i++) lineRenderer.SetPosition(i, selectedNodes[i].transform.position);
        }

        private void HandleInputReleased() { if (selectedNodes.Count >= 2) ProcessMerge(); else ClearSelection(); }

        private void ProcessMerge()
        {
            int totalValue = 0;
            foreach (var node in selectedNodes) totalValue += node.Value;
            int resultValue = Mathf.ClosestPowerOfTwo(totalValue);

            // --- SKOR EKLEME ---
            if (ScoreManager.Instance != null)
                ScoreManager.Instance.AddScore(resultValue);

            NumberNode lastNode = selectedNodes[selectedNodes.Count - 1];
            Vector3 targetPos = lastNode.transform.position;

            for (int i = 0; i < selectedNodes.Count - 1; i++)
            {
                gridManager.RemoveNode(selectedNodes[i].GridX, selectedNodes[i].GridY);
                selectedNodes[i].MergeInto(targetPos, null);
            }
            StartCoroutine(FinalizeMerge(lastNode, resultValue));
        }

        private System.Collections.IEnumerator FinalizeMerge(NumberNode lastNode, int resultValue)
        {
            yield return new WaitForSeconds(0.15f);
            if (lastNode != null)
            {
                lastNode.SetValue(resultValue, numberData.GetColorForValue(resultValue));
                lastNode.PlayPopEffect();
            }
            gridManager.FillHoles();
            // --- YENİ: Sayılar düştükten kısa bir süre sonra hamle kontrolü yap ---
            yield return new WaitForSeconds(0.3f);
            if (!gridManager.HasPossibleMoves())
            {
                Debug.Log("Oyun Bitti");
                FindAnyObjectByType<UIManager>().ShowGameOver();
            }
            ClearSelection();
        }

        private void ClearSelection() { selectedNodes.Clear(); currentLinkValue = -1; lineRenderer.positionCount = 0; }
    }
}