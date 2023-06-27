using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SpointMap : MonoBehaviour
{
    [SerializeField] private string filePath = "Assets/_Game/Map1.txt";
    public GameObject[] prefabs; // Mảng prefab tương ứng với các giá trị số trong tệp văn bản
    private string[,] mapData; // Mảng hai chiều để lưu dữ liệu bản đồ
    void Start()
    {
        LoadMapFromFile();
        GenerateMap();
    }
    private void LoadMapFromFile()
    {
        string[] lines = File.ReadAllLines(filePath);
        int rowCount = lines.Length;
        int columnCount = lines[0].Length;

        mapData = new string[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                string value = lines[row][col].ToString();
                mapData[row, col] = value;
            }
        }
    }

    private void GenerateMap()
    {
        int rowCount = mapData.GetLength(0);
        int columnCount = mapData.GetLength(1);

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                string value = mapData[row, col];

                if (int.TryParse(value, out int intValue) && intValue >= 0 && intValue < prefabs.Length)
                {
                    GameObject prefab = prefabs[intValue];
                    Vector3 position = new Vector3(col, 0f, row);
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }
    }
}
