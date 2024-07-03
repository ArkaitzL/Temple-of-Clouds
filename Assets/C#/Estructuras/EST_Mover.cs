using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EST_Mover : MonoBehaviour
{
    [SerializeField] float tiempoEspera = 2;
    [SerializeField] float velocidad = 2;
    [SerializeField] bool irAtras, inicioAuto;
    [Space]
    [SerializeField] Transform padre;
    [SerializeField] Vector3[] ruta;

    bool iniciado, atras;
    int index = 0;

    const float TAMAÑO_MARCADOR = 5f, DISTANCIA_LLEGADA = 0.5f;

    private void Start()
    {
        //Suscribirse al reinicio
        Movimiento.reiniciar += Reiniciar;

        //Validar que tenga una ruta
        if (ruta == null || ruta.Length == 0)  return;

        //Se pone en la posicion inicial
        transform.position = padre.position + ruta[index];
        if(inicioAuto) StartCoroutine(Mover());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && !inicioAuto && !iniciado) 
        {
            iniciado = true;
            StartCoroutine(Mover());
        }
    }

    IEnumerator Mover()
    {
        yield return new WaitForSeconds(tiempoEspera);

        //Calcula la direccion a la que se va a mover
        Vector3 objetivo = padre.position + ((atras)
            ? ruta[--index]
            : ruta[++index]);

        Vector3 direccion = -(transform.position - objetivo).normalized;

        //Se mueva a su proxima posicion
        while (Vector3.Distance(objetivo, transform.position) > DISTANCIA_LLEGADA)
        {
            transform.Translate(direccion * velocidad * Time.deltaTime, Space.World);
            yield return null;
        }
        transform.position = objetivo;

        //Reinicia el proceso
        if (irAtras)
        {
            if (index == ruta.Length-1)
            {
                atras = true;
                //index--;
            }
            if (index == 0)
            {
                atras = false;
                //index = 1;
            }
        }
        else
        {
            if (index ==ruta.Length) index = 0;
        }
        StartCoroutine(Mover());
    }

    public void Reiniciar()
    {
        iniciado = false;
        index = 0;
        transform.position = padre.position + ruta[index];
    }

    // Dibuja un círculo en cada punto de la ruta en el modo de edición cuando el objeto está seleccionado
    private void OnDrawGizmos()
    {
        if (ruta == null || ruta.Length == 0 || padre == null) return;

        Gizmos.color = Color.red;
        foreach (Vector3 punto in ruta)
        {
            Gizmos.DrawSphere(padre.position + punto, TAMAÑO_MARCADOR);
        }
    }

}
