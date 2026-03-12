using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Настройки кубов")]
    public int totalCubes = 5; // сколько всего кубов на карте
    private int cubesCollected = 0;

    [Header("UI Победы")]
    public GameObject winPanel; // панель с текстом и кнопками
    public Text winMessageText; // текст "Вы выиграли!"
    public Button restartButton; // кнопка "Заново"
    public Button exitButton; // кнопка "Выход"

    [Header("UI Счетчик (опционально)")]
    public Text cubesCounterText; // текст вида "Кубы: 0/5"

    void Start()
    {
        // Сначала выключаем панель победы
        if (winPanel != null)
            winPanel.SetActive(false);

        // Подключаем кнопки
        if (restartButton != null)
            restartButton.onClick.AddListener(RestartGame);

        if (exitButton != null)
            exitButton.onClick.AddListener(ExitGame);

        // Обновляем счетчик
        UpdateCubesCounter();
    }

    // Этот метод вызывается из куба, когда его собирают
    public void CollectCube()
    {
        cubesCollected++;
        Debug.Log($"Кубы: {cubesCollected}/{totalCubes}");

        // Обновляем счетчик на экране
        UpdateCubesCounter();

        // Проверяем, собраны ли все кубы
        if (cubesCollected >= totalCubes)
        {
            WinGame();
        }
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
        Debug.Log("🎉 ПОБЕДА! Все кубы собраны!");

        // Останавливаем игру
        Time.timeScale = 0f; // замораживаем время

        // Показываем панель победы
        if (winPanel != null)
        {
            winPanel.SetActive(true);

            // Можно изменить текст
            if (winMessageText != null)
                winMessageText.text = "ВЫ ВЫИГРАЛИ!";
        }

        // Отключаем управление игроком
        PlayerController player = FindObjectOfType<PlayerController>();
        if (player != null)
            player.enabled = false;
    }

    // Перезапуск игры
    public void RestartGame()
    {
        Time.timeScale = 1f; // возвращаем время
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // перезагружаем сцену
    }

    // Выход из игры
    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // для редактора
#else
            Application.Quit(); // для собранной игры
#endif
    }
}