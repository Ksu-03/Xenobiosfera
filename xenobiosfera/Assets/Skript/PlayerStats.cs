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
    public bool isDead = false;

    [Header("Смерть")]
    public GameObject deathEffect;
    public float respawnTime = 3f;
    public Vector3 respawnPosition = Vector3.zero;

    [Header("Звуки")]
    public AudioClip deathSound;
    public AudioClip oxygenAlarmSound;
    public AudioClip energyAlarmSound;

    [Header("События")]
    public Action OnOxygenEmpty;
    public Action OnEnergyEmpty;
    public Action OnPlayerDeath;

    private PlayerController playerController;
    private AudioSource audioSource;
    private bool oxygenAlarmPlaying = false;
    private bool energyAlarmPlaying = false;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
            audioSource = gameObject.AddComponent<AudioSource>();

        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return; // если мертвы - ничего не делаем

        UpdateOxygen();
        UpdateEnergy();

        // Проверка на смерть
        CheckDeathConditions();
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

        // Звук тревоги при низком кислороде
        if (oxygen.currentValue < 20 && !oxygenAlarmPlaying && oxygenAlarmSound != null)
        {
            audioSource.PlayOneShot(oxygenAlarmSound);
            oxygenAlarmPlaying = true;
        }
        else if (oxygen.currentValue >= 20)
        {
            oxygenAlarmPlaying = false;
        }

        if (oxygen.IsEmpty())
        {
            OnOxygenEmpty?.Invoke();
        }
    }

    void UpdateEnergy()
    {
        // Тратится только при беге
        if (isRunning)
        {
            energy.Update(true);
        }

        // Звук тревоги при низкой энергии
        if (energy.currentValue < 20 && !energyAlarmPlaying && energyAlarmSound != null)
        {
            audioSource.PlayOneShot(energyAlarmSound);
            energyAlarmPlaying = true;
        }
        else if (energy.currentValue >= 20)
        {
            energyAlarmPlaying = false;
        }

        if (energy.IsEmpty())
        {
            OnEnergyEmpty?.Invoke();

            if (playerController != null)
                playerController.canRun = false;
        }
        else
        {
            if (playerController != null)
                playerController.canRun = true;
        }
    }

    void CheckDeathConditions()
    {
        // Смерть от нехватки кислорода
        if (oxygen.IsEmpty())
        {
            Die("Кислород закончился!");
        }
        // Можно добавить другие условия смерти
        // if (health.IsEmpty()) Die("Здоровье на нуле!");
    }

void Die(string reason)
    {
        if (isDead) return;

        isDead = true;
        Debug.Log($"Смерть: {reason}");

        // Отключаем управление
        if (playerController != null)
            playerController.enabled = false;

        // Отключаем коллайдер
        GetComponent<Collider>().enabled = false;

        // Эффект смерти
        if (deathEffect != null)
            Instantiate(deathEffect, transform.position, Quaternion.identity);

        // Звук смерти
        if (deathSound != null)
            AudioSource.PlayClipAtPoint(deathSound, transform.position);

        // Вызываем событие
        OnPlayerDeath?.Invoke();

        // Запускаем респавн
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        // Восстанавливаем ресурсы
        oxygen.currentValue = oxygen.maxValue;
        energy.currentValue = energy.maxValue;

        // Возвращаем на точку респавна
        transform.position = respawnPosition;

        // Включаем компоненты
        if (playerController != null)
            playerController.enabled = true;

        GetComponent<Collider>().enabled = true;

        isDead = false;

        Debug.Log("Игрок воскрес!");
    }

    public void RestoreOxygen(float amount)
    {
        oxygen.currentValue = Mathf.Clamp(oxygen.currentValue + amount, 0, oxygen.maxValue);

        // Всплывающий текст
        if (FloatingText.Instance != null)
        {
            FloatingText.Instance.ShowText(
                $"+{amount} O₂",
                transform.position + Vector3.up * 2,
                Color.cyan
            );
        }
    }

    public void RestoreEnergy(float amount)
    {
        energy.currentValue = Mathf.Clamp(energy.currentValue + amount, 0, energy.maxValue);

        // Всплывающий текст
        if (FloatingText.Instance != null)
        {
            FloatingText.Instance.ShowText(
                $"+{amount} ⚡",
                transform.position + Vector3.up * 2,
                Color.yellow
            );
        }
    }
}
