using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EST_Caida : MonoBehaviour
{
    [SerializeField] GameObject entero, roto;

    Collider colicion;
    bool terminado;

    private void Start()
    {
        colicion = GetComponent<Collider>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !terminado)
        {
            colicion.enabled = false;
            terminado = true;

            entero.SetActive(false);
            roto.SetActive(true);
        }
    }
}
