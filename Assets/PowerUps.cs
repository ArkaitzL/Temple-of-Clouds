using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUps : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] float salto = 25.0f;

    Rigidbody rb;
    Movimiento mv;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<Movimiento>();
    }

    private void Update()
    {
        Saltar();
    }

    void Saltar()
    {
        if (!Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Space)) return;
        if (!mv.enSuelo()) return;

        rb.AddForce(Vector3.up * salto, ForceMode.Impulse);
    }

}
