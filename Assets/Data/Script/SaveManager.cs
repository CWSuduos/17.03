using UnityEngine;
using System.IO;
using System.Collections.Generic;

public class SaveManager : MonoBehaviour
{
    private string saveFilePath;

    private void Start()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "building_data.json");
    }

    public void SaveBuildingData(List<BuildingData> buildings)
    {
        string json = JsonUtility.ToJson(new BuildingDataListWrapper { buildings = buildings }, true);
        File.WriteAllText(saveFilePath, json);
    }

    public List<BuildingData> LoadBuildingData()
    {
        if (File.Exists(saveFilePath))
        {
            string json = File.ReadAllText(saveFilePath);
            BuildingDataListWrapper wrapper = JsonUtility.FromJson<BuildingDataListWrapper>(json);

            return wrapper.buildings;
        }
        else
        {
            return new List<BuildingData>();
        }
    }
}

[System.Serializable]
public class BuildingDataListWrapper
{
    public List<BuildingData> buildings;
}

