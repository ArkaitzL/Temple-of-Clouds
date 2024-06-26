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

        foreach (var h in Habilidades)
        {
            AñadirHabilidad(h, true);
            h.carga = 1;
        }
    }

    void Update()
    {
        //Reiniciar
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        PowerUps();
    }

    void PowerUps() 
    {
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

        if (!menu.activeSelf)
        {
            menu.SetActive(true);
            Time.timeScale = VELOCIDAD_TIEMPO;

            enMenu = true;
        }

        if (actual != -1) menu.transform.GetChild(actual).GetChild(0).gameObject.SetActive(false);

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

        menu.transform.GetChild(actual).GetChild(0).gameObject.SetActive(true);
        cambiarHabilidad?.Invoke(actual);
        habilidadActual.sprite = Habilidades[actual].imagen;
        RecargarUI(Habilidades[actual].carga);
    }

    public void AñadirHabilidad(UIPowerUp habilidad, bool existentes = false) 
    {
        GameObject elemento = Instantiate(icono, menu.transform);
        elemento.GetComponent<Image>().sprite = habilidad.imagen;
        if(!existentes) Habilidades.Add(habilidad);
    }

    public void GastarPowerUp(int index) 
    {
        if (Habilidades[index].gasto <= Habilidades[index].carga)
        {
            Habilidades[index].carga -= Habilidades[index].gasto;
            RecargarUI(Habilidades[index].carga);
        }
    }

    public void RecargarPowerUp() 
    {
        ControladorBG.Rutina(UPDATE_CARGA, () => 
        {
            foreach (var h in Habilidades)
            {
                if (h.carga >= MAX_CARGA) continue;

                h.carga += h.regargar;

                if (h.carga > MAX_CARGA) h.carga = MAX_CARGA;

                if(actual != -1 && h.nombre.Equals(Habilidades[actual].nombre)) RecargarUI(h.carga);
            }
        }, true);
    }

    void RecargarUI(float carga)
    {
        if (habilidadActualTiempo == null) return;

        habilidadActualTiempo.fillAmount = 1 - Mathf.Clamp01(carga / MAX_CARGA);
    }
}


