using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{


    [System.Serializable]
    public class Stat
    {
        public float currentValue;
        public float maxValue;
        public float drainRate;     // скорость траты в секунду
        public float regenRate;     // скорость восстановления в секунду

        public void Update(bool isDraining)
        {
            if (isDraining)
                currentValue = Mathf.Clamp(currentValue + drainRate * Time.deltaTime, 0, maxValue);
            else
                currentValue = Mathf.Clamp(currentValue + regenRate * Time.deltaTime, 0, maxValue);
        }

        public bool IsEmpty()
        {
            return currentValue <= 0;
        }
    }

    [Header("Основные показатели")]
    public Stat oxygen = new Stat() { currentValue = 100, maxValue = 100, drainRate = -1f, regenRate = 0f };
    public Stat energy = new Stat() { currentValue = 100, maxValue = 100, drainRate = -0.5f, regenRate = 0f };

    [Header("Условия")]
    public bool isRunning = false;

    [Header("События")]
    public Action OnOxygenEmpty;    // используем System.Action
    public Action OnEnergyEmpty;    // используем System.Action

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();

        // Проверка на наличие PlayerController (не критично, если нет)
        if (playerController == null)
        {
            Debug.Log("PlayerStats: PlayerController не найден - это нормально, если вы его еще не добавили");
        }
    }

    void Update()
    {
        UpdateOxygen();
        UpdateEnergy();
    }

    void UpdateOxygen()
    {
        // Базовая трата
        oxygen.Update(true);

        // При беге тратится в 2 раза быстрее
        if (isRunning)
        {
            oxygen.currentValue = Mathf.Clamp(oxygen.currentValue + oxygen.drainRate * Time.deltaTime, 0, oxygen.maxValue);
        }

        if (oxygen.IsEmpty())
        {
            OnOxygenEmpty?.Invoke();
            Debug.Log("Кислород закончился!");
        }
    }

    void UpdateEnergy()
    {
        // Тратится только при беге
        if (isRunning)
        {
            energy.Update(true);
        }

        if (energy.IsEmpty())
        {
            OnEnergyEmpty?.Invoke();
            Debug.Log("Энергия закончилась!");

            if (playerController != null)
                playerController.canRun = false;
        }
        else
        {
            if (playerController != null)
                playerController.canRun = true;
        }
    }

    // Пополнение кислорода (вызывается из триггера)
    public void RestoreOxygen(float amount)
    {
        oxygen.currentValue = Mathf.Clamp(oxygen.currentValue + amount, 0, oxygen.maxValue);
        Debug.Log($"Кислород пополнен: +{amount}");
    }

    // Пополнение энергии (вызывается из триггера)
    public void RestoreEnergy(float amount)
    {
        energy.currentValue = Mathf.Clamp(energy.currentValue + amount, 0, energy.maxValue);
        Debug.Log($"Энергия пополнена: +{amount}");
    }
}
