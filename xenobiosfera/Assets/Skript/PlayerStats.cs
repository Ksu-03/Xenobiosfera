using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;

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

    [Header("Здоровье")]
    public float health = 100f;
    public float maxHealth = 100f;

    [Header("Респавн")]
    public float respawnTime = 5f;
    public Vector3 respawnPosition;

    [Header("UI")]
    public GameObject gameOverPanel;

    [Header("Состояние")]
    public bool isDead = false;

    private PlayerController playerController;
    private GameManager gameManager;

    void Start()
    {
        playerController = GetComponent<PlayerController>();
        respawnPosition = transform.position;
        gameManager = FindObjectOfType<GameManager>();

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (isDead) return;

        // Кислород
        oxygen -= oxygenDrainRate * Time.deltaTime;
        oxygen = Mathf.Clamp(oxygen, 0, maxOxygen);

        // Энергия
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

        if (oxygen <= 0 || health <= 0)
        {
            Die();
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        health = Mathf.Clamp(health, 0, maxHealth);

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

        if (playerController != null)
            playerController.enabled = false;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = false;

        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);

            Button[] buttons = gameOverPanel.GetComponentsInChildren<Button>();
            foreach (Button btn in buttons)
            {
                btn.onClick.RemoveAllListeners();

                if (btn.name.Contains("Restart") || btn.name.Contains("Заново"))
                    btn.onClick.AddListener(Respawn);

                if (btn.name.Contains("Exit") || btn.name.Contains("Выход"))
                    btn.onClick.AddListener(ExitGame);
            }
        }

        Time.timeScale = 0f;
    }

    public void Respawn()
    {
        Time.timeScale = 1f;

        oxygen = maxOxygen;
        energy = maxEnergy;
        health = maxHealth;

        transform.position = respawnPosition;

        if (playerController != null)
            playerController.enabled = true;

        Collider col = GetComponent<Collider>();
        if (col != null)
            col.enabled = true;

        isDead = false;

        if (gameOverPanel != null)
            gameOverPanel.SetActive(false);

        Debug.Log("✨ ПЕРСОНАЖ ВОСКРЕС");

        // Возрождаем кубы
        if (gameManager != null)
        {
            gameManager.RespawnAllCubes();
        }
    }

    void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

    public void RestoreOxygen(float amount)
    {
        oxygen = Mathf.Clamp(oxygen + amount, 0, maxOxygen);
    }
    public void RestoreEnergy(float amount)
    {
        energy = Mathf.Clamp(energy + amount, 0, maxEnergy);
    }
    public void RestoreHealth(float amount)
    {
        health = Mathf.Clamp(health + amount, 0, maxHealth);
    }
}