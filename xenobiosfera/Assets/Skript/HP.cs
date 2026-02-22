using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HP : MonoBehaviour
{
    public float Health = 100f;
    public Slider HpBar;

    private void Update()
    {
        HpBar.value = Health;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            Health -= 10;
        }
    }
}
