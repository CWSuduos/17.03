using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    [Header("�������� ����������")]
    [SerializeField] private GridManager gridManager;
    [SerializeField] private BuildingPlacer buildingPlacer;
    [SerializeField] private SpawnChanger spawnChanger;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);

        LoadMainScene();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
