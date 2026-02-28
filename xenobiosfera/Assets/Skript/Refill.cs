using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refill : MonoBehaviour
{
    [Header("Количество")]
    public float oxygenAmount = 50f;
    public float energyAmount = 30f;

    [Header("Текст")]
    public string pickupMessage = "+Кислород!";
    public Color messageColor = Color.green;
    public bool showMessage = true;

    [Header("Эффекты")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    [Header("Настройки")]
    public string playerTag = "Player";
    public bool destroyOnPickup = true;
    public float respawnTime = 5f;

    // Для отслеживания состояния
    private bool isUsed = false;
    private Collider capsuleCollider;
    private MeshRenderer capsuleRenderer;

    void Start()
    {
        // Получаем компоненты при старте
        capsuleCollider = GetComponent<Collider>();
        capsuleRenderer = GetComponent<MeshRenderer>();

        // Проверяем, что коллайдер - триггер
        if (capsuleCollider != null && !capsuleCollider.isTrigger)
        {
            capsuleCollider.isTrigger = true;
            Debug.Log($"{gameObject.name}: коллайдер автоматически сделан триггером");
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Если уже использована - игнорируем
        if (isUsed) return;

        // Проверяем тег игрока
        if (other.CompareTag(playerTag))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                // Помечаем как использованную
                isUsed = true;

                string message = "";

                // Пополняем ресурсы
                if (oxygenAmount > 0)
                {
                    stats.RestoreOxygen(oxygenAmount);
                    message = $"+{oxygenAmount} O₂";
                }

                if (energyAmount > 0)
                {
                    stats.RestoreEnergy(energyAmount);
                    if (message != "")
                        message += $" +{energyAmount} ⚡";
                    else
                        message = $"+{energyAmount} ⚡";
                }

                // Показываем всплывающий текст
                if (showMessage && FloatingText.Instance != null)
                {
                    FloatingText.Instance.ShowText(
                        message,
                        transform.position + Vector3.up,
                        messageColor
                    );
                }

                // Эффекты
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                // Исчезаем
                if (destroyOnPickup)
                {
                    if (respawnTime > 0)
                    {
                        // Отключаем визуал и коллайдер
                        if (capsuleRenderer != null)
                            capsuleRenderer.enabled = false;

                        if (capsuleCollider != null)
                            capsuleCollider.enabled = false;

                        // Запускаем респавн
                        Invoke(nameof(RespawnCapsule), respawnTime);
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    void RespawnCapsule()
    {
        // Включаем обратно
        if (capsuleRenderer != null)
            capsuleRenderer.enabled = true;

if (capsuleCollider != null)
            capsuleCollider.enabled = true;

        isUsed = false;

        Debug.Log($"{gameObject.name}: перезарядилась!");
    }

    // Для отладки - показываем в редакторе, что это триггер
    private void OnDrawGizmos()
    {
        Collider col = GetComponent<Collider>();
        if (col != null && col.isTrigger)
        {
            Gizmos.color = new Color(0, 1, 0, 0.3f);
            Gizmos.DrawCube(col.bounds.center, col.bounds.size);
        }
    }
}