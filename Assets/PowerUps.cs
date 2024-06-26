using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUps : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] float salto = 25.0f;

    Rigidbody rb;
    Movimiento mv;

    Dictionary<string, Action> powerups;
    string actual;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<Movimiento>();

        powerups = new Dictionary<string, Action>{
            { "Salto", Saltar}, 
        };
    }

    private void Update()
    {
        if (!Input.GetMouseButtonDown(1) && !Input.GetKeyDown(KeyCode.Space)) return;
        powerups[actual]?.Invoke();
    }

    void Saltar()
    {
        if (!mv.enSuelo()) return;
        rb.AddForce(Vector3.up * salto, ForceMode.Impulse);
    }

}
