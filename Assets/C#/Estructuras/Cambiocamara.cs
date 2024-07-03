using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cambiocamara : MonoBehaviour
{
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaje.inst.RotarCam(
                (transform.position - other.transform.position).normalized
            );
        }
    }
}
