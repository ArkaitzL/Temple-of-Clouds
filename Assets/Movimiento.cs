using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

[RequireComponent(typeof(Rigidbody))]
public class Movimiento : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] public float velocidad = 15.0f;
    [SerializeField] float velocidadMax = 25.0f;

    [Header("Cuerpo")]
    [SerializeField] float altura = 1.0f;
    [SerializeField] LayerMask capaSuelo;

    [Header("Otros")]
    [SerializeField] MiCursor cursor;
    [SerializeField] Transform motor;
    [SerializeField] [Range(-1, 1)] int direccion = -1;

    Rigidbody rb;
    Quaternion motor_rotacion;

    const float DISTANCIA_MUERTE = -25, DISTANCIA_RESPAWN = 100;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        motor_rotacion = motor.rotation;

        //Empezar en el ultimo checkpoint
        transform.position = Save.Data.ultimoCheckpoint.Y(DISTANCIA_RESPAWN);

    }

    void Update()
    {
        Unir();
        Rotar();
        Morir();
    }

    void Unir() 
    {
        //Se asegura de que la REF no se mueva
        motor.rotation = motor_rotacion;
    }

    void Rotar() 
    {
        //Rota hacia donde apunta el raton
        Vector3 centro = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 direccion = Input.mousePosition - centro;

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        angulo = Mathf.Repeat(angulo + 90f, 360f);
        cursor.Rotar(angulo);

        motor.rotation = Quaternion.Euler(new Vector3(0, -angulo, 0));
        motor_rotacion = motor.rotation;
    }

    void Morir() 
    {
        //Respawnea en el checkpoint al morir
        if (transform.position.y <= DISTANCIA_MUERTE)
        {
            transform.position = Save.Data.ultimoCheckpoint.Y(DISTANCIA_RESPAWN);

            Vector3 velocidad = rb.velocity;
            velocidad.x = 0;
            velocidad.z = 0;

            rb.velocity = velocidad;
        }
    }

    void FixedUpdate()
    {
        Mover();
    }

    void Mover() 
    {
        if (!enSuelo()) return;

        //No esta moviendose
        if (!Input.GetMouseButton(0)) {
            cursor.Cambiar(false);
            return;
        }

        //Se mueve
        cursor.Cambiar(true);
        rb.AddForce(Direccion() * velocidad, ForceMode.Impulse);

        //Limita la velocidad
        if (rb.velocity.magnitude > velocidadMax)
        {
            rb.velocity = rb.velocity.normalized * velocidadMax;
        }
    }

    //Comprueba si esta tocando el suelo
    public bool enSuelo() => Physics.CheckSphere(transform.position, altura, capaSuelo);

    //Comprueba la direccion en la que se tiene que mover
    public Vector3 Direccion() => motor.forward * direccion;

    //Enseña la velocidad actual
    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), rb.velocity.magnitude.ToString());
    }

    //Enseña el detector del suelo
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, altura);
    }
}
