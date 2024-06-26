using System;
using UnityEngine;

[CreateAssetMenu(fileName = "UIPowerUp", menuName = "SO/UIPowerUp")]
[Serializable]
public class UIPowerUp : ScriptableObject
{
    public string nombre;
    public Sprite imagen;
    public float gasto; // Gasto por click
    public float regargar; // Recarga por 0.1 segundos
    [HideInInspector] public float carga; // Carga de la habilidad
}
