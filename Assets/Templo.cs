using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Templo : MonoBehaviour
{
    [SerializeField] UIPowerUp habilidad;

    private void Start()
    {
        //** CAMBIAR PARA QUE LO HAGA AL INTERACTUAR CON EL TEMPLO ** //
        //** SI YA EXISTE NO SE LA DA DE NUEVO **// -> APAGAR TEMPLO
        UI.inst.AñadirHabilidad(habilidad);
    }
}
