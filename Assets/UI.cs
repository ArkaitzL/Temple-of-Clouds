using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using BaboOnLite;

public class UI : MonoBehaviour
{
    [Header("PowerUps")]
    [SerializeField] GameObject menu;
    [SerializeField] GameObject icono;
    [SerializeField] Image habilidadActual, habilidadActualTiempo;

    [HideInInspector] public Action<int> cambiarHabilidad;
    [HideInInspector] public bool enMenu;
    int actual = -1;

    public static UI inst;

    const float ESPERA_SALIR_MENU = 0.1f, VELOCIDAD_TIEMPO = 0.25f; // CAMARA LENTA
    const float MAX_CARGA = 1f, UPDATE_CARGA = 0.1f; // RECARGAR HABILIDADES

    public List<UIPowerUp> Habilidades { get => Save.Data.habilidades; set => Save.Data.habilidades = value; }

    private void Awake()
    {  
        inst = this;
    }

    private void Start()
    {
        RecargarPowerUp();

        //Actualiza las habilidades guardadas anteriormente
        for (int i = 0; i < Habilidades.Count; i++)
        {
            AñadirHabilidad(Habilidades[i]);
            Habilidades[i].carga = 1;
        }
    }

    void Update()
    {
        //Reiniciar
        if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        PowerUps();
    }

    void PowerUps() 
    {
        //Sale del menu con Click Derecho
        if (enMenu && Input.GetMouseButtonDown(1)) {

            Time.timeScale = 1f;
            menu.SetActive(false);

            ControladorBG.Rutina(ESPERA_SALIR_MENU, () => {
                enMenu = false;
            });
        }

        if (Habilidades == null || Habilidades.Count == 0) return;

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0) return;

        //Entra en el menu
        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            Time.timeScale = VELOCIDAD_TIEMPO;

            enMenu = true;
        }

        //Desactiva habilidad anterior
        if (actual != -1) menu.transform.GetChild(actual).GetChild(0).gameObject.SetActive(false);

        //Comprueba hacia que direccion a girado la rueda del raton
        if (scroll > 0f)
        {
            // Desplazar hacia arriba
            actual--;
            if (actual < 0)
            {
                actual = Habilidades.Count - 1;
            }
        }
        else if (scroll < 0f)
        {
            // Desplazar hacia abajo
            actual++;
            if (actual >= Habilidades.Count)
            {
                actual = 0;
            }
        }

        //Cambia habilidad actual
        menu.transform.GetChild(actual).GetChild(0).gameObject.SetActive(true);
        cambiarHabilidad?.Invoke(actual);
        habilidadActual.sprite = Habilidades[actual].imagen;
        RecargarUI();
    }

    public void AñadirHabilidad(UIPowerUp habilidad) 
    {
        //Añade una nueva habilidad a la lista
        GameObject elemento = Instantiate(icono, menu.transform);
        habilidad.imagenCarga = elemento.transform.GetChild(1).GetComponent<Image>();
        Image imagen = elemento.GetComponent<Image>();
        imagen.sprite = habilidad.imagen;
        imagen.color = habilidad.color;

        //Reemplaza la habilidad existente
        int index = Habilidades.FindIndex(h=> h.nombre == habilidad.nombre);
        if (index != -1)
        {
            Habilidades[index] = habilidad;
            return;
        }

        //Añade una nueva habilidad
        Habilidades.Add(habilidad);

    }

    public void GastarPowerUp(int index) 
    {
        //Consume la carga
        if (Habilidades[index].gasto <= Habilidades[index].carga)
        {
            Habilidades[index].carga -= Habilidades[index].gasto;
            RecargarUI();
        }
    }

    public void RecargarPowerUp() 
    {
        //Recarga constantemente la carga
        ControladorBG.Rutina(UPDATE_CARGA, () => 
        {
            foreach (var h in Habilidades)
            {
                if (h.carga >= MAX_CARGA) continue;

                h.carga += h.regargar;

                if (h.carga > MAX_CARGA) h.carga = MAX_CARGA;
            }

            RecargarUI(false);
        }, true);
    }

    void RecargarUI(bool soloActual = true)
    {
        //Actualiza la UI de los PowerUps
        if (habilidadActualTiempo == null || actual == -1) return;

        habilidadActualTiempo.fillAmount = 1 - Mathf.Clamp01(Habilidades[actual].carga / MAX_CARGA);

        if (soloActual) return;

        foreach (var h in Habilidades)
        {
            h.imagenCarga.fillAmount = 1 - Mathf.Clamp01(h.carga / MAX_CARGA);
        }


    }
}


