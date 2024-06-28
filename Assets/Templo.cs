using BaboOnLite;
using System;
using UnityEngine;

public class Templo : MonoBehaviour
{
    [SerializeField] public bool checkpoint = true, info, powerup;

    [Space]

    //CheckPoint
    [SerializeField] int id;
    [SerializeField] Renderer marcadorCheckpoint;
    [SerializeField] Material apagado, encendido;
    public static event Action ultimoCheckpoint;

    //PowerUp
    [SerializeField] UIPowerUp habilidad;
    [SerializeField] Renderer marcadorPowerup;

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

    private void Start()
    {
        if (checkpoint)
        {
            marcadorCheckpoint.material = apagado;

            ultimoCheckpoint += ApagarCheckPoint;
            if (Save.Data.ultimoCheckpointID == id) marcadorCheckpoint.material = encendido;
        }

        if (powerup)
        {
            marcadorPowerup.material.color = habilidad.color;
        }
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
            PowerUps();
        }
        else if (info)
        {

        }

    }

    void PowerUps() 
    {
        UI.inst.AñadirHabilidad(habilidad);
    }

    void CheckPoint() 
    {
        //Apaga el marcado anterior
        if (Save.Data.ultimoCheckpointID != -1)
        {
            ultimoCheckpoint?.Invoke();
        }

        //Enciende y cambia de marcador
        Save.Data.ultimoCheckpointID = id;
        marcadorCheckpoint.material = encendido;

        //Guarda la ubicacion
        Save.Data.ultimoCheckpoint = transform.position;
    }

    void ApagarCheckPoint() 
    {
        //Activar el checkpoint actual
        if (Save.Data.ultimoCheckpointID == id) marcadorCheckpoint.material = apagado;
    }
}
