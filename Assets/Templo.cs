using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class Templo : MonoBehaviour
{
    [SerializeField] public bool checkpoint = true, info, powerup;

    [Space]

    //CheckPoint
    [SerializeField] Renderer marcador;
    [SerializeField] Material apagado, encendido;

    //PowerUp
    [SerializeField] UIPowerUp habilidad;

    //Info - PowerUp
    [SerializeField] Transform informacion;


    public void OnValidate()
    {
        if (checkpoint)
        {
            info = false;
            powerup = false;
        }
        else if (info)
        {
            checkpoint = false;
            powerup = false;
        }
        else if (powerup)
        {
            checkpoint = false;
            info = false;
        }

        //Forzar el refresco del inspector
        UnityEditor.EditorUtility.SetDirty(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;

        if (checkpoint)
        {
            CheckPoint();
        }
        else if (powerup)
        {
            UI.inst.AñadirHabilidad(habilidad);
        }
        else if (info)
        {

        }

    }

    void CheckPoint() 
    {
        //Apaga el marcado anterior
        if (Save.Data.ultimoCheckpointMarcador != null)
        {
            Save.Data.ultimoCheckpointMarcador.material = apagado;
        }

        //Enciende y cambia de marcador
        Save.Data.ultimoCheckpointMarcador = marcador;
        marcador.material = encendido;

        //Guarda la ubicacion
        Save.Data.ultimoCheckpoint = transform.position;
    }
}
