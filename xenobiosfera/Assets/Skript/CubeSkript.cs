using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.PostProcessing;

public class CubeSkript : MonoBehaviour
{
    [SerializeField] Text TextCountCubes;
    [SerializeField] Text TakeCube;
    [SerializeField] int countCubes;

    private void Start()
    {
        countCubes = GameObject.FindGameObjectsWithTag("Cube").Length;
        TakeCube.gameObject.SetActive(false);
    }

    private void Update()
    {
        UpdateText();
    }

    void UpdateText()
    {
        TextCountCubes.text = "Cubs: " + countCubes;
    }

    private void OnTriggeraStay(Collider other)
    {
        if (other.CompareTag("Cube"))
        {
            TakeCube.gameObject.SetActive(true);
            if (Input.GetKeyDown(KeyCode.V))
            {
                TakeCube.gameObject.SetActive(false);
                Destroy(other.gameObject);
                countCubes--;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        TakeCube.gameObject.SetActive(false);
    }
}
