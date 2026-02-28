using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingText : MonoBehaviour
{
    public static FloatingText Instance;

    [Header("Настройки")]
    public GameObject textPrefab;
    public Canvas canvas;
    public float textDuration = 1.5f;
    public float floatSpeed = 50f;

    void Awake()
    {
        Instance = this;
    }

    public void ShowText(string message, Vector3 worldPosition, Color color)
    {
        if (textPrefab == null || canvas == null)
        {
            Debug.LogWarning("FloatingText: не назначен префаб или канвас!");
            return;
        }

        // Создаем текст
        GameObject textObj = Instantiate(textPrefab, canvas.transform);
        Text text = textObj.GetComponent<Text>();

        if (text != null)
        {
            text.text = message;
            text.color = color;
        }

        // Конвертируем мировые координаты в экранные
        Vector2 screenPosition = Camera.main.WorldToScreenPoint(worldPosition);
        textObj.GetComponent<RectTransform>().position = screenPosition;

        // Запускаем анимацию
        StartCoroutine(AnimateText(textObj));
    }

    System.Collections.IEnumerator AnimateText(GameObject textObj)
    {
        float elapsedTime = 0;
        RectTransform rect = textObj.GetComponent<RectTransform>();
        Text text = textObj.GetComponent<Text>();
        Color startColor = text.color;

        while (elapsedTime < textDuration)
        {
            // Поднимаем вверх
            rect.position += Vector3.up * floatSpeed * Time.deltaTime;

            // Исчезаем
            float alpha = 1 - (elapsedTime / textDuration);
            text.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            elapsedTime += Time.deltaTime;
            yield return null;
        }

        Destroy(textObj);
    }
}
