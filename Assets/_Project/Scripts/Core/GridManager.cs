using UnityEngine;
 using Game.Data;

 namespace Game.Core
{
    public class GridManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private GridSettings settings;
        [SerializeField] private GameObject cellPrefab;
        [Header("References")]
        [SerializeField] private Transform gridParent;

        void Start()
        {
            GenerateGrid();
        }
        private void GenerateGrid()
        {
            // Izgarayı Ekranın Tam Ortasına Hizalamak İçin Ofset Hesaplıyoruz
            float totalWidth = (settings.width - 1) * settings.cellSize;
            float totalHeight = (settings.height - 1) * settings.cellSize;
            Vector3 startPosition = new Vector3(-totalWidth / 2f , -totalHeight / 2f , 0);
            for(int x = 0; x < settings.width; x++)
            {
                for(int y = 0; y < settings.height; y++)
                {
                    Vector3 spawnPos = startPosition + new Vector3(x * settings.cellSize , y * settings.cellSize , 0);
                    GameObject newCell = Instantiate(cellPrefab , spawnPos , Quaternion.identity , gridParent);
                    newCell.name = $"Cell_{x}_{y}";
                }
            }
        }
    }
}