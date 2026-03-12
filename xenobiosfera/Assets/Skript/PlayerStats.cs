using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerStats : MonoBehaviour
{
    [Header("Кислород")]
    public float oxygen = 100f;
    public float maxOxygen = 100f;
    public float oxygenDrainRate = 1f;

    [Header("Энергия")]
    public float energy = 100f;
    public float maxEnergy = 100f;
    public float energyDrainRate = 0.5f;

    [Header("Здоровье")] // Добавили здоровье
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Респавн")]
    public float respawnTime = 5f; // теперь 5 секунд (было 2)
    public Vector3 respawnPosition;

    [Header("Состояние")]
    public bool isDead = false;

    private PlayerController playerController;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        respawnPosition = transform.position;
    }

    void Update()
    {
        if (isDead) return;

        // --- КИСЛОРОД ---
        oxygen -= oxygenDrainRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);

        // --- ЭНЕРГИЯ ---
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        if (Mathf.Abs(horizontal) > 0.1f || Mathf.Abs(vertical) > 0.1f)
        {
            energy -= energyDrainRate * Time.deltaTime;
        }

        energy = Mathf.Clamp(energy, 0, maxEnergy);

       
        if (playerController != null)
        {
            playerController.canMove = (energy > 0 && !isDead);
        }

        // --- ПРОВЕРКА НА СМЕРТЬ ---
        if (oxygen <= 0 || health <= 0 || energy <= 0)
        {
            Die();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            health -= 10;

        }
    }

    // МЕТОД ДЛЯ ПОЛУЧЕНИЯ УРОНА - вызывать из мониторов
    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

        Debug.Log($"💔 Получен урон: {amount}, Здоровье: {health}");

        // Проверка на смерть от урона
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        if (isDead) return;

        isDead = true;
        Debug.Log("💀 ПЕРСОНАЖ УМЕР");

        // Отключаем управление
        if (playerController != null)
            playerController.enabled = false;

        // Отключаем коллайдер
        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        // Респавн через respawnTime секунд
        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        // Восстанавливаем всё
        oxygen = maxOxygen;
        energy = maxEnergy;
        health = maxHealth; // здоровье тоже восстанавливаем

        // Возвращаем на старт
        transform.position = respawnPosition;

        // Включаем всё обратно
        if (playerController != null)
            playerController.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        isDead = false;
        Debug.Log("✨ ПЕРСОНАЖ ВОСКРЕС");
    }

    // Методы для пополнения (капсулы)
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

    public void RestoreHealth(float amount) // если будут аптечки
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
        Debug.Log($"Здоровье +{amount}");
    }
}
