using UnityEngine;
using Game.Data;
using Game.Gameplay;
using System.Collections.Generic;

namespace Game.Core
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private GridSettings settings;
        [SerializeField] private NumberData numberData;
        [SerializeField] private GameObject cellPrefab;
        [SerializeField] private GameObject nodePrefab;
        [SerializeField] private Transform gridParent;

        private NumberNode[,] nodes;

        private void Start()
        {
            nodes = new NumberNode[settings.width, settings.height];
            GenerateGrid();
        }

        private void GenerateGrid()
        {
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    Vector3 spawnPos = CalculateWorldPosition(x, y);
                    Instantiate(cellPrefab, spawnPos, Quaternion.identity, gridParent);
                    CreateNode(spawnPos, x, y);
                }
            }
        }

        private void CreateNode(Vector3 position, int x, int y)
        {
            GameObject nodeObj = Instantiate(nodePrefab, position, Quaternion.identity, gridParent);
            NumberNode node = nodeObj.GetComponent<NumberNode>();
            node.SetGridPosition(x, y);
            nodes[x, y] = node;

            int startValue = GetRandomStartValue();
            node.SetValue(startValue, numberData.GetColorForValue(startValue));
        }

        // --- YENİ: Yerçekimi ve Boşluk Doldurma ---
        public void FillHoles()
        {
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    if (nodes[x, y] == null) // Boş hücre bulduk
                    {
                        for (int k = y + 1; k < settings.height; k++) // Üstündeki ilk dolu hücreyi ara
                        {
                            if (nodes[x, k] != null)
                            {
                                NumberNode nodeToMove = nodes[x, k];
                                nodes[x, y] = nodeToMove;
                                nodes[x, k] = null;

                                nodeToMove.SetGridPosition(x, y);
                                nodeToMove.MoveToPosition(CalculateWorldPosition(x, y));
                                break;
                            }
                        }
                    }
                }
            }
            SpawnNewNodes();
        }

        private void SpawnNewNodes()
        {
            for (int x = 0; x < settings.width; x++)
            {
                for (int y = 0; y < settings.height; y++)
                {
                    if (nodes[x, y] == null)
                    {
                        // Ekranın üstünden düşerek gelsin
                        Vector3 spawnPos = CalculateWorldPosition(x, y) + Vector3.up * 5f;
                        CreateNode(spawnPos, x, y);
                        nodes[x, y].MoveToPosition(CalculateWorldPosition(x, y));
                    }
                }
            }
        }

        public Vector3 CalculateWorldPosition(int x, int y)
        {
            float totalWidth = (settings.width - 1) * settings.cellSize;
            float totalHeight = (settings.height - 1) * settings.cellSize;
            Vector3 startPosition = new Vector3(-totalWidth / 2f, -totalHeight / 2f, 0);
            return startPosition + new Vector3(x * settings.cellSize, y * settings.cellSize, 0);
        }

        private int GetRandomStartValue()
        {
            int[] values = { 2, 4, 8 };
            return values[Random.Range(0, values.Length)];
        }

        public void RemoveNode(int x, int y) { nodes[x, y] = null; }
        public bool HasPossibleMoves()
        {
            for(int x = 0; x < settings.width; x++)
            {
                for(int y = 0; y < settings.height; y++)
                {
                    if(nodes[x , y] == null) continue;
                    int currentValue = nodes[x , y].Value;
                    // Sadece Sağ Ve Üst Komşulara Bakmak Yeterlidir (Çift Kontrolü Önler)
                    Vector2Int[] directions = {Vector2Int.right , Vector2Int.up};
                    foreach(var dir in directions)
                    {
                        int nextX = x + dir.x;
                        int nextY = y + dir.y;
                        if(nextX < settings.width && nextY < settings.height)
                        {
                            if(nodes[nextX , nextY] != null && nodes[nextX , nextY].Value == currentValue)
                            {
                                return true; // En Bir Hamşle Bulundu
                            }
                        }
                    }
                }
            }
            return false; // Hiç Hamle Kalmadı 
        }
    }
}