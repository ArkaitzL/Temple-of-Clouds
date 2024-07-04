using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

[Serializable]
public partial class SaveScript
{
    //----------------------------------------------------------------//
    //Variables por defecto: Estas varibles se usan automaticamente   //
    //----------------------------------------------------------------//
    public int lenguaje = 0;

    //-----------------------------------------------------------------
    public List<UIPowerUp> habilidades = new List<UIPowerUp>(); // Habilidades obtenidas
    public Vector3 ultimoCheckpoint = Vector3.zero; //Posicion de spawn
    public int ultimoCheckpointID = -1; //Referencia al ultimo checkpoint
    public int habilidadActual = -1; // Habilidad en uso
    public float ultimoAngulo = 180; //Rotacion de la camara en el checkpoint
}
