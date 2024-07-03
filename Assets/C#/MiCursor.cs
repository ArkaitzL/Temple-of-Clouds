using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiCursor : MonoBehaviour
{
    [SerializeField] Sprite desactivado, activado;
    Image imagen;
    bool activo;

    private void Start()
    {
        Cursor.visible = false;
        imagen = GetComponent<Image>();
    }

    private void Update()
    {
        //Sige al raton con la imagen del raton
        Vector3 posicion = Input.mousePosition;
        posicion.z = 0;
        transform.position = posicion;
    }

    public void Cambiar(bool cambiar) 
    {
        //Se aseura de no cambiar al mismo estado en el que estaba
        if (cambiar == activo) return;

        //Cambia de imagen entre vacio y lleno
        activo = cambiar;
        imagen.sprite = (activo) ? activado : desactivado;
    }

    public void Rotar(float angulo) 
    {
        //Rota el cursor hacia donde se va a mover
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 180));
    }
}
