using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;
using System;

[RequireComponent(typeof(Rigidbody))]
public class Personaje : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] public float velocidad = 15.0f;
    [SerializeField] float velocidadMax = 25.0f;

    [Header("Cuerpo")]
    [SerializeField] float altura = 1.0f, anchura;
    [SerializeField] LayerMask capaSuelo;

    [Header("Otros")]
    [SerializeField] MiCursor cursor;
    [SerializeField] Transform motor, camara;
    [SerializeField] [Range(-1, 1)] int direccion = -1;

    Quaternion motor_rotacion, camara_rotacion;
    Rigidbody rb;
    [HideInInspector] public float anguloCam = 180;

    public static event Action reiniciar;
    public static Personaje inst;

    const float DISTANCIA_MUERTE = -25, DISTANCIA_RESPAWN = 25, UMBRAL_CAM = 0.5f;

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        motor_rotacion = motor.rotation;
        camara_rotacion = camara.rotation;

        //Empezar en el ultimo checkpoint
        transform.position = Save.Data.ultimoCheckpoint.Y(DISTANCIA_RESPAWN);

        //Empieza con el angolo correcto
        RotarCam();
    }

    void Update()
    {
        Unir();
        Rotar();
        Morir();

        //Desactiva el curso cuando no este en el suelo
        if (!enSuelo()) cursor.Cambiar(false);
    }

    void Unir() 
    {
        //Se asegura de que la REF no se mueva
        motor.rotation = motor_rotacion;
        camara.rotation = camara_rotacion;
    }

    void Rotar() 
    {
        Vector3 centro = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);

        // Convertir la dirección del mouse a coordenadas del mundo
        Vector3 posicionRaton = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        Vector3 posicionMotor = Camera.main.ScreenToWorldPoint(new Vector3(centro.x, centro.y, Camera.main.nearClipPlane));

        // Calcular la dirección en el espacio del mundo
        Vector3 direccion = posicionRaton - posicionMotor;
        direccion.y = 0; // Asegúrate de que la dirección esté en el plano horizontal

        // Calcular el ángulo en el espacio del mundo
        float angulo = Mathf.Atan2(-direccion.z, direccion.x) * Mathf.Rad2Deg;
        angulo = Mathf.Repeat(angulo - 90f, 360f); // Ajustar el ángulo para que sea relativo a la dirección hacia adelante del motor

        cursor.Rotar(angulo + anguloCam);

        motor.rotation = Quaternion.Euler(new Vector3(0, angulo, 0));
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
            reiniciar?.Invoke();

            //Cambia el angulo de la camara
            RotarCam();
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

    public void RotarCam(Vector3 direccion) 
    {
        (Vector3, float)[] direcciones = {
            (Vector3.forward, 180f),
            (Vector3.back, 0f),
            (Vector3.right, -90f),
            (Vector3.left, 90f)
        };

        foreach (var (dir, angulo) in direcciones)
        {
            if (Vector3.Dot(direccion, dir) > UMBRAL_CAM)
            {
                camara.rotation = Quaternion.Euler(0, angulo, 0);
                anguloCam = angulo;

                if (anguloCam == 0 || anguloCam == 180) anguloCam += 180;
                if (anguloCam == 360) anguloCam = 0;

                break;
            }
        }

        camara_rotacion = camara.rotation;
    }

    public void RotarCam() 
    {
        anguloCam = Save.Data.ultimoAngulo;
        float cambio = 0;

        if (anguloCam == 0 || anguloCam == 180) cambio = 180;
        //if (cambio == 360) cambio = 0;

        camara.rotation = Quaternion.Euler(0, anguloCam + cambio, 0);
        camara_rotacion = camara.rotation;
    }

    //Comprueba si esta tocando el suelo
    public bool enSuelo()
    {
        Vector3 centroCaja = transform.position + Vector3.down * (altura / 2);
        return Physics.CheckBox(centroCaja, new Vector3(anchura, altura, anchura) / 2, Quaternion.identity, capaSuelo);
    }

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
        Vector3 centroCaja = transform.position + Vector3.down * (altura / 2);
        Gizmos.DrawWireCube(centroCaja, new Vector3(anchura, altura, anchura));
    }
}
