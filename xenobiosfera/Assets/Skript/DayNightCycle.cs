using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycle : MonoBehaviour
{
    [Header("Настройки времени")]
    [Tooltip("Скорость вращения солнца (1 = реальное время)")]
    public float timeSpeed = 1.0f;

    [Tooltip("Текущее время суток (0-24)")]
    public float currentTime = 12.0f;

    private void Update()
    {
        // Увеличиваем время
        currentTime += Time.deltaTime * timeSpeed;
        if (currentTime >= 24.0f) currentTime = 0.0f;

        // Вращаем солнце (Directional Light) вокруг оси X
        // 0-12 часов: восход/день, 12-24: закат/ночь
        float rotation = (currentTime / 24.0f) * 360.0f - 90.0f;
        transform.rotation = Quaternion.Euler(rotation, -30.0f, 0.0f);

    }
}
