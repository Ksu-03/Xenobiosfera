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

    [Header("Текстовые значения")]
    public Text oxygenText;
    public Text energyText;

    [Header("Цвета")]
    public Color oxygenColor = Color.green;
    public Color energyColor = Color.yellow;
    public Color criticalColor = Color.red;
    public Color backgroundColor = new Color(0.2f, 0.2f, 0.2f);

    [Header("Критические значения")]
    public float criticalThreshold = 20f;
    public float blinkSpeed = 5f;

    private Image oxygenFill;
    private Image energyFill;
    private Color originalOxygenColor;
    private Color originalEnergyColor;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        // Настройка слайдеров
        if (oxygenSlider != null)
        {
            oxygenSlider.maxValue = playerStats.maxOxygen;
            oxygenFill = oxygenSlider.fillRect.GetComponent<Image>();
            if (oxygenFill != null)
            {
                oxygenFill.color = oxygenColor;
                originalOxygenColor = oxygenColor;
            }

            // Настройка фона
            SetBackgroundColor(oxygenSlider, backgroundColor);
        }

        if (energySlider != null)
        {
            energySlider.maxValue = playerStats.maxEnergy;
            energyFill = energySlider.fillRect.GetComponent<Image>();
            if (energyFill != null)
            {
                energyFill.color = energyColor;
                originalEnergyColor = energyColor;
            }

            SetBackgroundColor(energySlider, backgroundColor);
        }
    }

    void SetBackgroundColor(Slider slider, Color color)
    {
        Transform background = slider.transform.Find("Background");
        if (background != null)
        {
            Image bgImage = background.GetComponent<Image>();
            if (bgImage != null)
                bgImage.color = color;
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

        // Обновляем текст
        if (oxygenText != null)
            oxygenText.text = $"O₂: {playerStats.oxygen:F0}/{playerStats.maxOxygen}";

        if (energyText != null)
            energyText.text = $"⚡: {playerStats.energy:F0}/{playerStats.maxEnergy}";

        // Мигание при критическом уровне
        UpdateCriticalStates();
    }

    void UpdateCriticalStates()
    {
        // Кислород
        if (oxygenFill != null)
        {
            if (playerStats.oxygen < criticalThreshold)
            {
                float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1) / 2;
                oxygenFill.color = Color.Lerp(oxygenColor, criticalColor, alpha);
            }
            else
            {
                oxygenFill.color = oxygenColor;
            }
        }

        // Энергия
        if (energyFill != null)
        {
            if (playerStats.energy < criticalThreshold)
            {
                float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1) / 2;
                energyFill.color = Color.Lerp(energyColor, criticalColor, alpha);
            }
            else
            {
                energyFill.color = energyColor;
            }
        }
    }
}