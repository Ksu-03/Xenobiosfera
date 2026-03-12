using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    [Header("Ссылка на игрока")]
    public PlayerStats playerStats;

    [Header("Слайдеры")]
    public Slider oxygenSlider;
    public Slider energySlider;
    public Slider healthSlider; // добавили слайдер здоровья

    [Header("Текстовые значения")]
    public Text oxygenText;
    public Text energyText;
    public Text healthText; // добавили текст здоровья

    [Header("Цвета")]
    public Color oxygenColor = Color.green;
    public Color energyColor = Color.yellow;
    public Color healthColor = Color.red; // цвет здоровья
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f);

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        // Настройка слайдеров
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = playerStats.maxOxygen;
            SetSliderColor(oxygenSlider, oxygenColor);
        }

        if (energySlider != null)
        {
            energySlider.maxValue = playerStats.maxEnergy;
            SetSliderColor(energySlider, energyColor);
        }

        if (healthSlider != null) // настройка слайдера здоровья
        {
            healthSlider.maxValue = playerStats.maxHealth;
            SetSliderColor(healthSlider, healthColor);
        }
    }

    void SetSliderColor(Slider slider, Color fillColor)
    {
        // Заливка
        Image fillImage = slider.fillRect.GetComponent<Image>();
        if (fillImage != null)
            fillImage.color = fillColor;

        // Фон
        Transform background = slider.transform.Find("Background");
        if (background != null)
        {
            Image bgImage = background.GetComponent<Image>();
            if (bgImage != null)
                bgImage.color = backgroundColor;
        }
    }

    void Update()
    {
        if (playerStats == null) return;

        // Обновляем значения
        if (oxygenSlider != null)
            oxygenSlider.value = playerStats.oxygen;

        if (energySlider != null)
            energySlider.value = playerStats.energy;

        if (healthSlider != null) // обновляем здоровье
            healthSlider.value = playerStats.health;

        // Обновляем текст
        if (oxygenText != null)
            oxygenText.text = $"O₂: {playerStats.oxygen:F0}/{playerStats.maxOxygen}";

        if (energyText != null)
            energyText.text = $"⚡: {playerStats.energy:F0}/{playerStats.maxEnergy}";

        if (healthText != null) // текст здоровья
            healthText.text = $"❤: {playerStats.health:F0}/{playerStats.maxHealth}";
    }
}