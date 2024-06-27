using System;
using UnityEngine;
using UnityEngine.UI;

//Clase para guardar los datos de los POWERUPS que se ven en el UI
[CreateAssetMenu(fileName = "UIPowerUp", menuName = "SO/UIPowerUp")]
public class UIPowerUp : ScriptableObject
{
    public string nombre;
    public Sprite imagen;
    public float gasto; // Gasto por click
    public float regargar; // Recarga por 0.1 segundos
    [HideInInspector] public float carga = 1; // Carga de la habilidad
    [HideInInspector] public Image imagenCarga;
}
