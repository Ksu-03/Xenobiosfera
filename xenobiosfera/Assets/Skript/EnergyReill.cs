using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnergyReill : MonoBehaviour
{
    [Header("Настройки пополнения")]
    public float refillAmount = 50f;
    public bool destroyOnTouch = false;
    public string targetTag = "Player";

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag))
        {
            PlayerStats stats = other.GetComponent<PlayerStats>();

            if (stats != null)
            {
                stats.RestoreEnergy(refillAmount);

                if (destroyOnTouch)
                {
                    Destroy(gameObject);
                }

                Debug.Log($"энергия на {refillAmount} от {gameObject.name}");
            }
        }
    }
}

