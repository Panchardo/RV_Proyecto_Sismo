using UnityEngine;
using System.Collections;

public class DetectorTrigger : MonoBehaviour
{
    public SimuladorSismo scriptPrincipal; // Acá arrastrás el objeto con el SimuladorSismo
    public ControlCabeza scriptCabeza;
    private bool yaSeUso = false;
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player") && !yaSeUso)
        {
            yaSeUso = true;
            StartCoroutine(TemporizadorSismo());
        }
    }
    IEnumerator TemporizadorSismo()
    {
        scriptPrincipal.enZonaTerremoto = true;
        scriptCabeza.haySismo = true;
        yield return new WaitForSeconds(10.0f); // Duración fija según el INPRES
        scriptPrincipal.enZonaTerremoto = false;
        scriptCabeza.haySismo = false;
    }
/*
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            scriptPrincipal.enZonaTerremoto = false;
            scriptCabeza.haySismo = false;
        }
    }
*/
}