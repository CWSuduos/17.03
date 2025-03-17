using UnityEngine;
public class GridManager : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private Vector3 startPosition = Vector3.zero;
    [SerializeField] private int gridSize = 10;
    [SerializeField] private float cellSize = 5f;
    [SerializeField] private float spacing = 0.5f;
    GameObject[,] grid;
    public float CellSize => cellSize;
    public float Spacing => spacing;
    public GameObject[,] Grid => grid;
    public int GridSize => gridSize;
    public Vector3 StartPosition => startPosition;
    void Start() { GenerateGrid(); }
    void GenerateGrid()
    {
        grid = new GameObject[gridSize, gridSize];
        for (int x = 0; x < gridSize; x++)
        {
            for (int z = 0; z < gridSize; z++)
            {
                Vector3 position = startPosition + new Vector3(x * (cellSize + spacing), 0, z * (cellSize + spacing));
                GameObject cell = Instantiate(cellPrefab, position, Quaternion.identity, transform);
                grid[x, z] = cell;
                cell.tag = "ValidCell";
            }
        }
    }
    public Vector3 GetCellPositionById(int x, int z)
    {
        float xPos = x * (cellSize + spacing);
        float zPos = z * (cellSize + spacing);
        return startPosition + new Vector3(xPos, 0, zPos);
    }
}
