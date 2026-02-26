using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenRefill : MonoBehaviour

{
    [Header("Настройки пополнения")]
    public float refillAmount = 50f;        // сколько кислорода добавлять
    public bool destroyOnTouch = false;      // исчезать ли после использования
    public string targetTag = "Player";      // тег игрока

    private void OnTriggerEnter(Collider other)
    {
        // Проверяем, что вошедший объект - игрок
        if (other.CompareTag(targetTag))
        {
            // Пытаемся получить компонент PlayerStats
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                // Пополняем кислород
                stats.RestoreOxygen(refillAmount);

                // Если нужно, уничтожаем объект
                if (destroyOnTouch)
                {
                    Destroy(gameObject);
                }
                else
                {
                    // Опционально: отключаем коллайдер на время перезарядки
                     GetComponent<Collider>().enabled = false;
                     Invoke(nameof(EnableCollider), 5f); // включить через 5 сек
                }

                Debug.Log($"Игрок пополнил кислород на {refillAmount} от {gameObject.name}");
            }
        }
    }

   
    void EnableCollider()
    {
        GetComponent<Collider>().enabled = true;
    }
}

