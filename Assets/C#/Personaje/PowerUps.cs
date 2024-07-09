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
    Personaje mv;

    //PowerUps
    Dictionary<string, Action> powerups;
    int actual = -1;

    public static bool disponible = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        mv = GetComponent<Personaje>();

        UI.inst.cambiarHabilidad = CambiarHabilidad; //Manda la funcion desde donde se cambia la habilidad actual

        //Lista con todos los PowerUps *Tiene que tener los mismos que en los SO con el mismo nombre*
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
        if (UI.inst.enMenu || actual == -1) return; //Comprueba que no este en el menu y que tenga una habilidad en uso

        //Llama a la habilidad actual
        UIPowerUp powerActual = UI.inst.Habilidades[actual];
        if (powerActual.gasto <= powerActual.carga)
        {
            powerups[powerActual.nombre]?.Invoke();
        }
    }

    //Cambia la habilidad actual
    void CambiarHabilidad(int index)
    {
        actual = index;
    }

    //POWER UPS FUNCIONES

    void Saltar()
    {
        if (!disponible || !Input.GetMouseButtonDown(1)) return;
        if (!mv.enSuelo()) return;

        rb.AddForce(Vector3.up * salto, ForceMode.Impulse);
        UI.inst.GastarPowerUp(actual); //Consume el PowerUp
    }

    void Velocidad() 
    {
        if (disponible && Input.GetMouseButton(1) && mv.enSuelo())
        {
            mv.velocidad = velocidadDef * incremento;
            UI.inst.GastarPowerUp(actual); //Consume el PowerUp
        }
        else {
            mv.velocidad = velocidadDef;
        }
    }

    void Dash()
    {
        if (!disponible || !Input.GetMouseButtonDown(1)) return;

        rb.AddForce(mv.Direccion() * impulso, ForceMode.Impulse);
        UI.inst.GastarPowerUp(actual); //Consume el PowerUp
    }
}
