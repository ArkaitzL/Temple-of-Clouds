using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BaboOnLite;

public class CheckPoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Save.Data.ultimoCheckpoint = transform.position;
        }
    }
}
