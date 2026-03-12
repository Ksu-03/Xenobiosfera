using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CubeSkript : MonoBehaviour
{ 
    [Header("Настройки")]
    public string playerTag = "Player";
    public GameObject pickupEffect; // эффект при сборе (можно не ставить)
    public AudioClip pickupSound; // звук при сборе (можно не ставить)

    [Header("Визуал")]
    public float rotationSpeed = 50f; // чтобы куб красиво крутился

    void Update()
    {
        // Просто красивое вращение
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            // Находим менеджер игры и сообщаем о сборе
            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.CollectCube();
            }

            // Эффекты
            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Уничтожаем куб
            Destroy(gameObject);

            Debug.Log("Куб собран!");
        }
    }
}
