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
        Vector3 posicion = Input.mousePosition;
        posicion.z = 0;
        transform.position = posicion;
    }

    public void Cambiar(bool cambiar) 
    {
        if (cambiar == activo) return;

        activo = cambiar;
        imagen.sprite = (activo) ? activado : desactivado;
    }

    public void Rotar(float angulo) 
    {
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angulo - 180));
    }
}
