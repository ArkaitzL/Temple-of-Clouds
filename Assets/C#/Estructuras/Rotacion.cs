using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotacion : MonoBehaviour
{
    [SerializeField] float velocidad = 5f;
    [SerializeField] Vector3 direccion = Vector3.forward;

    void Update()
    {
        transform.Rotate(direccion * velocidad * Time.deltaTime);
    }
}
