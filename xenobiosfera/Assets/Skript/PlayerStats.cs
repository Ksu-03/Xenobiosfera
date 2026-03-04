using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Показатели")]
    public float oxygen = 100f;
    public float maxOxygen = 100f;
    public float oxygenDrainRate = 1f; // кислород уходит всегда

    public float energy = 100f;
    public float maxEnergy = 100f;
    public float energyDrainRate = 0.5f; // энергия уходит при движении

    [Header("Состояние")]
    public bool isDead = false;
    public Vector3 respawnPosition;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        // --- КИСЛОРОД (падает всегда) ---
        oxygen -= oxygenDrainRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);

        // --- ЭНЕРГИЯ (падает при движении WASD) ---
        float horizontal = Input.GetAxis("Horizontal"); // A и D
        float vertical = Input.GetAxis("Vertical");     // W и S

        // Если игрок движется в любую сторону
        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            energy -= energyDrainRate * Time.deltaTime;

            // Для отладки (можно убрать потом)
            // Debug.Log($"Энергия: {energy:F1}");
        }

        energy = Mathf.Clamp(energy, 0, maxEnergy);

        // --- БЛОКИРОВКА ДВИЖЕНИЯ ПРИ 0 ЭНЕРГИИ ---
        if (playerController != null)
        {
            playerController.canMove = (energy > 0);
        }

        // --- СМЕРТЬ ---
        if (oxygen <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log("💀 Персонаж умер (кислород кончился)");

        // Отключаем управление
        if (playerController != null)
            playerController.enabled = false;

        // Отключаем коллайдер
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Респавн через 2 секунды
        Invoke("Respawn", 2f);
    }

    void Respawn()
    {
        // Восстанавливаем ресурсы
        oxygen = maxOxygen;
        energy = maxEnergy;

        // Возвращаем на старт
        transform.position = respawnPosition;

        // Включаем всё обратно
        if (playerController != null)
            playerController.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        isDead = false;
        Debug.Log("✨ Персонаж воскрес");
    }

    // Для капсул
    public void RestoreOxygen(float amount)
    {
        oxygen = Mathf.Clamp(oxygen + amount, 0, maxOxygen);
        Debug.Log($"Кислород +{amount}");
    }

    public void RestoreEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, maxEnergy);
        Debug.Log($"Энергия +{amount}");
    }
}

