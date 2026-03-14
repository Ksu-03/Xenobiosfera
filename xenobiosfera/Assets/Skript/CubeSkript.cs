using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CollectibleCube : MonoBehaviour
{
    [Header("Настройки")]
    public string playerTag = "Player";
    public GameObject pickupEffect;
    public AudioClip pickupSound;

    [Header("Визуал")]
    public float rotationSpeed = 50f;

    private bool isCollected = false; // флаг, чтобы собрать только один раз

    void Start()
    {
        if (string.IsNullOrEmpty(gameObject.tag))
            gameObject.tag = "Cube";
    }

    void Update()
    {
        transform.Rotate(Vector3.up * rotationSpeed * Time.deltaTime);
    }

    void OnTriggerEnter(Collider other)
    {
        // Если уже собрали - игнорируем
        if (isCollected) return;

        if (other.CompareTag(playerTag))
        {
            // Сразу ставим флаг, чтобы не собрать второй раз
            isCollected = true;

            GameManager gameManager = FindObjectOfType<GameManager>();
            if (gameManager != null)
            {
                gameManager.CollectCube();
            }

            if (pickupEffect != null)
                Instantiate(pickupEffect, transform.position, Quaternion.identity);

            if (pickupSound != null)
                AudioSource.PlayClipAtPoint(pickupSound, transform.position);

            // Отключаем коллайдер, чтобы больше не срабатывал
            GetComponent<Collider>().enabled = false;

            // Отключаем визуал (чтобы куб исчез)
            GetComponent<MeshRenderer>().enabled = false;

            // Уничтожаем через секунду (чтобы успели проиграться эффекты)
            Destroy(gameObject, 1f);

            Debug.Log("✅ Куб собран!");
        }
    }
}