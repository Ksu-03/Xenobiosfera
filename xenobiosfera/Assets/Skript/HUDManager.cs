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

    [Header("Текстовые значения (опционально)")]
    public Text oxygenText;
    public Text energyText;

    [Header("Цвета для слайдеров")]
    public Color oxygenColor = Color.green;
    public Color energyColor = Color.yellow;

    [Header("Анимация при критических значениях")]
    public float criticalThreshold = 20f;
    public float blinkSpeed = 5f;

    private Image oxygenFill;
    private Image energyFill;

    void Start()
    {
        if (playerStats == null)
            playerStats = FindObjectOfType<PlayerStats>();

        if (oxygenSlider != null)
            oxygenFill = oxygenSlider.fillRect.GetComponent<Image>();

        if (energySlider != null)
            energyFill = energySlider.fillRect.GetComponent<Image>();

        SetSliderMaxValues();
    }

    void SetSliderMaxValues()
    {
        if (oxygenSlider != null) oxygenSlider.maxValue = playerStats.oxygen.maxValue;
        if (energySlider != null) energySlider.maxValue = playerStats.energy.maxValue;
    }

    void Update()
    {
        if (playerStats == null) return;

        UpdateSlider(oxygenSlider, playerStats.oxygen.currentValue);
        UpdateSlider(energySlider, playerStats.energy.currentValue);

        UpdateText(oxygenText, playerStats.oxygen.currentValue, playerStats.oxygen.maxValue, "O₂");
        UpdateText(energyText, playerStats.energy.currentValue, playerStats.energy.maxValue, "⚡");

        HandleCriticalStates();
    }

    void UpdateSlider(Slider slider, float value)
    {
        if (slider != null)
            slider.value = value;
    }

    void UpdateText(Text text, float current, float max, string prefix)
    {
        if (text != null)
            text.text = $"{prefix}: {current:F0}/{max:F0}";
    }

    void HandleCriticalStates()
    {
        if (oxygenFill != null)
        {
            if (playerStats.oxygen.currentValue < criticalThreshold)
            {
                float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1) / 2;
                oxygenFill.color = new Color(1, 0, 0, alpha);
            }
            else
            {
                oxygenFill.color = oxygenColor;
            }
        }

        if (energyFill != null)
        {
            if (playerStats.energy.currentValue < criticalThreshold)
            {
                float alpha = (Mathf.Sin(Time.time * blinkSpeed) + 1) / 2;
                energyFill.color = new Color(1, 0, 0, alpha);
            }
            else
            {
                energyFill.color = energyColor;
            }
        }
    }
}
