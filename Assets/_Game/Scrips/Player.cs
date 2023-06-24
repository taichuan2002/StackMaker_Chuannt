using System.Buffers.Text;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject[] prefabs; // Mảng prefab tương ứng với các giá trị số trong tệp văn bản
    [SerializeField] private float speed;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private LayerMask unBrickBlock;
    [SerializeField] private Vector3 StartPoint;
    [SerializeField] public GameObject brickPrefab;
    [SerializeField] public GameObject wallPrefab;
    [SerializeField] public GameObject cubePrefab;
    [SerializeField] private string filePath = "Assets/_Game/Map1.txt";
    [SerializeField] private int countBrick;
    private int[,] mapData; // Mảng hai chiều để lưu dữ liệu bản đồ

    private float brickHeight;
    private bool isMoving = false;
    private Vector3 currentPos;
    private Vector3 targetPos;

    private void Start()
    {
        LoadMapFromFile();
        GenerateMap();
        countBrick = 0;
        brickHeight = 0.3f;
        transform.position = new Vector3(StartPoint.x, StartPoint.y, StartPoint.z);
    }

    private void Update()
    {
        Control();
    }
    private void LoadMapFromFile()
    {
        string[] lines = File.ReadAllLines(filePath);
        int rowCount = lines.Length;
        int columnCount = lines[0].Length;

        mapData = new int[rowCount, columnCount];

        for (int row = 0; row < rowCount; row++)
        {
            for (int col = 0; col < columnCount; col++)
            {
                int value = int.Parse(lines[row][col].ToString());
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
                int value = mapData[row, col];

                if (value >= 0 && value < prefabs.Length)
                {
                    GameObject prefab = prefabs[value];
                    Vector3 position = new Vector3(col, 0f, row);
                    Instantiate(prefab, position, Quaternion.identity);
                }
            }
        }
    }

    private void Control()
    {
        if (!isMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                currentPos = Input.mousePosition;
            }
            else if (Input.GetMouseButtonUp(0))
            {
                targetPos = Input.mousePosition;
                Vector3 moveDir = targetPos - currentPos;
                moveDir = GetDirection(moveDir);
                if (moveDir != Vector3.zero)
                {
                    StopAllCoroutines();
                    StartCoroutine(Move(moveDir));
                    StartCoroutine(CheckWall(moveDir));
                }
            }
        }
    }

    private Vector3 GetDirection(Vector3 direction)
    {
        float horizontal = Mathf.RoundToInt(direction.x);
        float vertical = Mathf.RoundToInt(direction.y);

        if (Mathf.Abs(horizontal) > Mathf.Abs(vertical))
        {
            return new Vector3(horizontal * 3f, 0f, 0f).normalized;
        }
        else if (Mathf.Abs(vertical) > Mathf.Abs(horizontal))
        {
            return new Vector3(0f, 0f, vertical * 3f).normalized;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private IEnumerator Move(Vector3 direction)
    {
        while (true)
        {
            isMoving = true;
            Vector3 movement = new Vector3(direction.x, 0f, direction.z);
            transform.position += movement * speed * Time.deltaTime;
            yield return null;
        }
    }

    private IEnumerator CheckWall(Vector3 direction)
    {
        while (true)
        {
            RaycastHit hit;
            Vector3 raycastPos = transform.position;
            if (Physics.Raycast(raycastPos, direction, out hit, 0.5f, wallLayer))
            {
                StopAllCoroutines();
                isMoving = false;
            }
            yield return null;
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.CompareTag("BrickBlock"))
        {
            countBrick += 1;
            Destroy(collision.gameObject);
            
        }
        if (collision.CompareTag("StopPoint"))
        {
            StopAllCoroutines();
            isMoving = false;
            collision.gameObject.SetActive(false);
        }

    }

/*    private void SpawnPrefabsFromText()
    {
        string fileContent = LoadTextFile(filePath);
        Debug.Log(fileContent);
        if (!string.IsNullOrEmpty(fileContent))
        {
            string[] lines = fileContent.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                int number;
                if (int.TryParse(lines[i], out number))
                {
                    GameObject prefab = GetPrefabByNumber(number);
                    if (prefab != null)
                    {
                        Vector3 position = new Vector3(i, 0, 0);
                        Instantiate(prefab, position, Quaternion.identity);
                    }
                }
                else
                {
                    Debug.LogError("Invalid number format: " + lines[i]);
                }
            }
        }
        else
        {
            Debug.LogError("Failed to load file: " + filePath);
        }
    }

    private GameObject GetPrefabByNumber(int number)
    {
        string prefabPath = "";

        switch (number)
        {
            case 0:
                prefabPath = "Prefabs/BrickBlock";
                break;
            case 1:
                prefabPath = "Prefabs/Wall";
                break;
            case 2:
                prefabPath = "Prefabs/Cube";
                break;
            default:
                Debug.LogError("Prefab not found for number: " + number);
                break;
        }

        if (!string.IsNullOrEmpty(prefabPath))
        {
            GameObject prefab = Resources.Load<GameObject>(prefabPath);
            if (prefab == null)
            {
                Debug.LogError("Failed to load prefab at path: " + prefabPath);
            }
            return prefab;
        }

        return null;
    }

    private string LoadTextFile(string filePath)
    {
        string fileContent = "";

        try
        {
            fileContent = File.ReadAllText(filePath);
        }
        catch (IOException e)
        {
            Debug.LogError("Failed to load text file: " + filePath + "\n" + e.Message);
        }

        return fileContent;
    }
*/

}
