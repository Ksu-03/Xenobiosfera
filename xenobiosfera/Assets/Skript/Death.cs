using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using UnityEngine;

public class Death : MonoBehaviour
{
    public GameObject cam1;
    public HP hp;

    private void Start()
    {
        cam1.gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            if (hp.Health <= 0)
            {
                Destroy(other.gameObject);
            }
        }
    }
}
