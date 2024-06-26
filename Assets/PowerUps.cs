using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PowerUps : MonoBehaviour
{
    [Header("Salto")]
    [SerializeField] float salto = 750.0f;

    [Header("Velocidad")]
    [SerializeField] float incremento = 2.0f;
    float velocidadDef;

    [Header("Dash")]
    [SerializeField] float impulso = 3000.0f;

    Rigidbody rb;
    Movimiento mv;

    Dictionary<string, Action> powerups;
    int actual = -1;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<Movimiento>();

        UI.inst.cambiarHabilidad = CambiarHabilidad;

        powerups = new Dictionary<string, Action>{
            {"Salto", Saltar},
            {"Velocidad", Velocidad},
            {"Dash", Dash},
        };

        //Velocidad
        velocidadDef = mv.velocidad;
    }

    private void Update()
    {
        if (UI.inst.enMenu || actual == -1) return;

        UIPowerUp powerActual = UI.inst.habilidades[actual];
        if (powerActual.gasto <= powerActual.carga)
        {
            powerups[powerActual.nombre]?.Invoke();
        }
    }

    void CambiarHabilidad(int index)
    {
        actual = index;
    }

    void Saltar()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        if (!mv.enSuelo()) return;

        rb.AddForce(Vector3.up * salto, ForceMode.Impulse);
        UI.inst.GastarPowerUp(actual);
    }

    void Velocidad() 
    {
        if (Input.GetMouseButton(1) && mv.enSuelo())
        {
            mv.velocidad = velocidadDef * incremento;
            UI.inst.GastarPowerUp(actual);
        }
        else {
            mv.velocidad = velocidadDef;
        }
    }

    void Dash()
    {
        if (!Input.GetMouseButtonDown(1)) return;
        rb.AddForce(mv.Direccion() * impulso, ForceMode.Impulse);
        UI.inst.GastarPowerUp(actual);
    }
}
