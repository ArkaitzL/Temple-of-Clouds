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
    [SerializeField] GameObject informacion;


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

        #if UNITY_EDITOR
        // Forzar el refresco del inspector
        UnityEditor.EditorUtility.SetDirty(this);
        #endif
    }

    private void Start()
    {
        if (checkpoint)
        {
            marcadorCheckpoint.material = apagado;

            ultimoCheckpoint += ApagarCheckPoint;
            if (Save.Data.ultimoCheckpointID == id) marcadorCheckpoint.material = encendido;
        }
        else if (powerup)
        {
            marcadorPowerup.material.color = habilidad.color;
        }
    }

    private void Update()
    {
        if (info || powerup)
        {
            CerrarInfo();
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
            AbrirInfo();
        }
        else if (info)
        {
            AbrirInfo();
        }

    }

    void AbrirInfo() 
    {
        informacion.SetActive(true);
        Time.timeScale = 0;
    }

    void CerrarInfo() 
    {
        if (Time.timeScale == 0 && Input.GetMouseButtonDown(1))
        {
            informacion.SetActive(false);
            Time.timeScale = 1;
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

    [ContextMenu("Usar")]
    public void Usar() {

        if (checkpoint)
        {
            Personaje.inst.transform.position = transform.position.Y(5);
            CheckPoint();
        }
        else if (powerup) 
        {
            PowerUps();
        }
    
    }
}
