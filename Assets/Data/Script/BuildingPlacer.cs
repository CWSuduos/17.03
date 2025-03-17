using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class BuildingPlacer : MonoBehaviour
{
    [SerializeField] private GridManager gridManager;
    [SerializeField] private List<BuildingData> defaultBuildings;
    [SerializeField] private Button confirmButton;
    [SerializeField] private Button deleteModeButton;
    [SerializeField] private SaveManager saveManager;  // Ссылка на менеджер сохранений

    private GameObject buildingToPlace;
    private BuildingData currentBuildingData;
    private bool canPlaceBuildings = false;
    private bool deleteMode = false;
    private List<BuildingData> placedBuildings = new List<BuildingData>();

    void Start()
    {
        confirmButton.onClick.AddListener(EnableBuildingPlacement);
        deleteModeButton.onClick.AddListener(ToggleDeleteMode);
        if (defaultBuildings.Count > 0) SetBuildingToPlace(defaultBuildings[0].prefab);

        // Загрузка ранее размещенных зданий
        placedBuildings = saveManager.LoadBuildingData();
    }

    public void EnableBuildingPlacement()
    {
        canPlaceBuildings = true;
        deleteMode = false;
        Debug.Log("Режим строительства активирован.");
    }

    public void ToggleDeleteMode()
    {
        deleteMode = !deleteMode;
        canPlaceBuildings = false;
        Debug.Log(deleteMode ? "Режим удаления включен." : "Режим удаления выключен.");
    }

    public void SetBuildingToPlace(GameObject buildingPrefab)
    {
        currentBuildingData = defaultBuildings.Find(b => b.prefab == buildingPrefab);
        buildingToPlace = buildingPrefab;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (deleteMode) TryDeleteBuilding(hit.collider.gameObject);
                else if (canPlaceBuildings) TryPlaceBuilding(hit.point);
            }
        }
    }

    void TryPlaceBuilding(Vector3 hitPoint)
    {
        int x = Mathf.FloorToInt((hitPoint.x - gridManager.StartPosition.x) / (gridManager.CellSize + gridManager.Spacing));
        int z = Mathf.FloorToInt((hitPoint.z - gridManager.StartPosition.z) / (gridManager.CellSize + gridManager.Spacing));
        if (x >= 0 && x < gridManager.GridSize && z >= 0 && z < gridManager.GridSize)
        {
            GameObject cell = gridManager.Grid[x, z];
            if (cell.CompareTag("ValidCell"))
            {
                Vector3 position = gridManager.GetCellPositionById(x, z);
                position.x += gridManager.CellSize * 0.5f;
                position.z += gridManager.CellSize * 0.5f;
                Vector3 buildingSize = new Vector3(currentBuildingData.width * gridManager.CellSize, 10f, currentBuildingData.length * gridManager.CellSize);
                Collider[] colliders = Physics.OverlapBox(position, buildingSize * 0.5f);
                foreach (Collider collider in colliders)
                {
                    if (collider.CompareTag("OccupiedCell"))
                    {
                        Debug.Log("Невозможно разместить здание, клетка занята.");
                        return;
                    }
                }
                PlaceBuildingAt(x, z, buildingSize);
            }
            else Debug.Log("Невалидная клетка.");
        }
    }

    void TryDeleteBuilding(GameObject clickedObject)
    {
        if (clickedObject.CompareTag("OccupiedCell"))
        {
            Destroy(clickedObject);
            Debug.Log("Здание удалено.");
        }
    }

    public void PlaceBuildingAt(int x, int z, Vector3 buildingSize)
    {
        Vector3 position = gridManager.GetCellPositionById(x, z);
        position.x += gridManager.CellSize * 0.5f;
        position.z += gridManager.CellSize * 0.5f;
        GameObject building = Instantiate(buildingToPlace, position, Quaternion.identity);
        building.tag = "OccupiedCell";
        building.transform.localScale = buildingSize;

        // Сохраняем информацию о здании
        BuildingData placedBuildingData = new BuildingData()
        {
            prefab = buildingToPlace,
            width = currentBuildingData.width,
            length = currentBuildingData.length,
            gridX = x,
            gridZ = z,
            position = building.transform.position,
            buildingName = buildingToPlace.name
        };

        placedBuildings.Add(placedBuildingData);

        // Сохраняем данные
        saveManager.SaveBuildingData(placedBuildings);

        Debug.Log("Здание размещено на клетке: " + x + ", " + z);
    }

    public void SaveBuildings()
    {
        saveManager.SaveBuildingData(placedBuildings);
    }
}
