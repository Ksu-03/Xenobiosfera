using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Refill : MonoBehaviour
{
    [Header("Количество")]
    public float oxygenAmount = 50f;
    public float energyAmount = 30f;

    [Header("Эффекты")]
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    [Header("Настройки")]
    public string playerTag = "Player";
    public bool destroyOnPickup = true;
    public float respawnTime = 0f; // 0 = не респавнится

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();
            if (stats != null)
            {
                // Пополняем ресурсы
                if (oxygenAmount > 0)
                    stats.RestoreOxygen(oxygenAmount);

                if (energyAmount > 0)
                    stats.RestoreEnergy(energyAmount);

                // Эффекты
                if (pickupEffect != null)
                    Instantiate(pickupEffect, transform.position, Quaternion.identity);

                if (pickupSound != null)
                    AudioSource.PlayClipAtPoint(pickupSound, transform.position);

                // Исчезаем или респавнимся
                if (destroyOnPickup)
                {
                    if (respawnTime > 0)
                    {
                        // Временно отключаем и запускаем таймер
                        gameObject.SetActive(false);
                        Invoke("RespawnCapsule", respawnTime); // Используем строку вместо nameof
                    }
                    else
                    {
                        Destroy(gameObject);
                    }
                }
            }
        }
    }

    // Метод для респавна - ОБЯЗАТЕЛЬНО должен быть
    void RespawnCapsule()
    {
        gameObject.SetActive(true);
        Debug.Log("Капсула перезарядилась!");
    }
}
