using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Movimiento : MonoBehaviour
{
    [Header("Movimiento")]
    [SerializeField] float velocidad = 15.0f;
    [SerializeField] float velocidadMax = 25.0f;

    [Header("Cuerpo")]
    [SerializeField] float altura = 1.0f;
    [SerializeField] LayerMask capaSuelo;

    [Header("Otros")]
    [SerializeField] Transform motor;
    [SerializeField] [Range(-1, 1)] int direccion = -1;

    Rigidbody rb;
    Quaternion motor_rotacion;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();

        motor_rotacion = motor.rotation;
    }

    void Update()
    {
        Unir();
        Rotar();
    }

    void Unir() 
    {
        motor.rotation = motor_rotacion;
    }

    void Rotar() 
    {
        Vector3 centro = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
        Vector3 direccion = Input.mousePosition - centro;

        float angulo = Mathf.Atan2(direccion.y, direccion.x) * Mathf.Rad2Deg;
        angulo = Mathf.Repeat(angulo + 90f, 360f);

        motor.rotation = Quaternion.Euler(new Vector3(0, -angulo, 0));
        motor_rotacion = motor.rotation;
    }

    void FixedUpdate()
    {
        Mover();
    }

    void Mover() 
    {
        if (!enSuelo()) return;

        if (!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.W)) return;
        rb.AddForce((motor.forward * direccion) * velocidad, ForceMode.Impulse);

        if (rb.velocity.magnitude > velocidadMax)
        {
            rb.velocity = rb.velocity.normalized * velocidadMax;
        }
    }

    public bool enSuelo() => Physics.CheckSphere(transform.position, altura, capaSuelo);

    void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 200, 20), rb.velocity.magnitude.ToString());
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, altura);
    }
}
