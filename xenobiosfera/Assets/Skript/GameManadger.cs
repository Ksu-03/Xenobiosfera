using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameManager : MonoBehaviour
{
    [Header("Настройки кубов")]
    public int totalCubes = 5;
    private int cubesCollected = 0;

    [Header("Префабы")]
    public GameObject cubePrefab;
    public Transform[] cubeSpawnPoints;

    [Header("UI")]
    public GameObject winPanel;
    public Text cubesCounterText;
    public Button restartButton;
    public Button exitButton;

    private List<GameObject> activeCubes = new List<GameObject>();

    // Для отладки - запоминаем, какие кубы уже собраны
    private List<string> collectedCubeIDs = new List<string>();

    void Start()
    {
        if (winPanel != null)
            winPanel.SetActive(false);

        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        FindAllCubes();
    }

    void FindAllCubes()
    {
        activeCubes.Clear();
        GameObject[] cubes = GameObject.FindGameObjectsWithTag("Cube");
        foreach (GameObject cube in cubes)
        {
            activeCubes.Add(cube);
        }

        totalCubes = activeCubes.Count;
        cubesCollected = 0;
        collectedCubeIDs.Clear();

        Debug.Log($"📊 Найдено кубов: {totalCubes}");
        UpdateCubesCounter();
    }

    public void CollectCube()
    {
        // Получаем имя объекта, который вызвал сбор (для отладки)
        string cubeName = "неизвестно";

        cubesCollected++;

        // ПРОВЕРКА: не даем превысить общее количество
        if (cubesCollected > totalCubes)
        {
            Debug.LogWarning($"⚠️ Попытка собрать больше кубов чем есть! {cubesCollected}/{totalCubes}");
            cubesCollected = totalCubes;
        }

        Debug.Log($"📦 Собрано кубов: {cubesCollected}/{totalCubes}");
        UpdateCubesCounter();

        if (cubesCollected >= totalCubes)
        {
            WinGame();
        }
    }

    public void RespawnAllCubes()
    {
        // Удаляем старые кубы
        foreach (GameObject cube in activeCubes)
        {
            if (cube != null)
                Destroy(cube);
        }
        activeCubes.Clear();

        // Создаем новые кубы
        if (cubePrefab != null && cubeSpawnPoints.Length > 0)
        {
            foreach (Transform spawnPoint in cubeSpawnPoints)
            {
                GameObject newCube = Instantiate(cubePrefab, spawnPoint.position, spawnPoint.rotation);
                activeCubes.Add(newCube);
            }
        }

        // Сбрасываем счетчик
        cubesCollected = 0;
        totalCubes = activeCubes.Count;
        collectedCubeIDs.Clear();

        UpdateCubesCounter();
        Debug.Log($"🔄 Кубы возрождены! Всего: {totalCubes}");
    }

    void UpdateCubesCounter()
    {
        if (cubesCounterText != null)
        {
            cubesCounterText.text = $"Кубы: {cubesCollected}/{totalCubes}";
        }
    }

    void WinGame()
    {
        Debug.Log("🎉 ПОБЕДА!");
        Time.timeScale = 0f;

        if (winPanel != null)
            winPanel.SetActive(true);

        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.enabled = false;
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}