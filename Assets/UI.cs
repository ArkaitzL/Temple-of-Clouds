using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UI : MonoBehaviour
{
    public List<UIPowerUp> habilidades;
    public static UI inst;

    private void Awake()
    {
        inst = this;
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
        
    }
}


