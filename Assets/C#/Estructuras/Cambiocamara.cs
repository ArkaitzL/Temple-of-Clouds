using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Cambiocamara : MonoBehaviour
{

    [SerializeField] bool checkpoint;

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Personaje.inst.RotarCam(
                (transform.position - other.transform.position).normalized
            );

            //Guarda el angulo de la camara
            if (checkpoint) Save.Data.ultimoAngulo = Personaje.inst.anguloCam;
        }
    }
}
