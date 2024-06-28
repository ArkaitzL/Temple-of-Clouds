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
    public Vector3 ultimoCheckpoint = Vector3.zero;
    public int ultimoCheckpointID = -1;
}
