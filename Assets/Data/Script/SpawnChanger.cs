using UnityEngine;
using UnityEngine.UI;

public class SpawnChanger : MonoBehaviour
{
    [SerializeField] private Button[] spawnButtons;
    [SerializeField] private GameObject[] prefabsToSpawn;
    [SerializeField] private BuildingPlacer buildingPlacer;

    void Start()
    {
        if (spawnButtons.Length != prefabsToSpawn.Length)
        {
            Debug.LogError("Количество кнопок и префабов должно быть одинаковым!");
            return;
        }

        for (int i = 0; i < spawnButtons.Length; i++)
        {
            int index = i; 
            spawnButtons[i].onClick.AddListener(() => SetBuilding(index));
        }
    }

    private void SetBuilding(int index)
    {
        if (index >= 0 && index < prefabsToSpawn.Length)
        {
            buildingPlacer.SetBuildingToPlace(prefabsToSpawn[index]);
            Debug.Log("Текущий префаб для размещения: " + prefabsToSpawn[index].name);
        }
    }
}
