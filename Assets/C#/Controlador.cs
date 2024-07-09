using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using TMPro;

public class Controlador : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI muertes, tiempo;

    void Start()
    {
        Personaje.reiniciar += Muerte;

        muertes.text = Save.Data.muertes.ToString();
        Tiempo();
    }

    private void Update()
    {
        Save.Data.tiempo += Time.deltaTime;
        Tiempo();
    }

    void Muerte() 
    {
        Save.Data.muertes++;
        muertes.text = Save.Data.muertes.ToString();
    }

    void Tiempo()
    {
        int minutos = Mathf.FloorToInt(Save.Data.tiempo / 60F);
        int segundos = Mathf.FloorToInt(Save.Data.tiempo % 60F);

        string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);
        tiempo.text = tiempoFormateado;
    }
}
