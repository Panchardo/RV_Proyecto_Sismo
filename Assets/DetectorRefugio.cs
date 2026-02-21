using UnityEngine;
using System.Collections;

public class DetectorRefugio : MonoBehaviour
{
    public ValvulaInteractiva[] valvulasDeEmergencia;
    public SimuladorSismo scriptPrincipal;
    public ControlCabeza scriptCabeza;
    public bool yaSeRefugio = false; // Mirá esta casilla en el Inspector al probar
    public MochilaEmergencia mochila;
    public PanelDialogo monitorOficina;
    void OnTriggerEnter(Collider other)
    {
        if (mochila.getMochilardaLista()){
        // Si lo que tocamos tiene la etiqueta correcta...
            if (other.CompareTag("ZonaSegura"))
            {
                yaSeRefugio = true;
                if (scriptPrincipal.enZonaTerremoto)
                {
                  StartCoroutine(SecuenciaRefugio());  
                }
                
                Debug.Log("✅ ¡ESTAS A SALVO!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Si salimos de la etiqueta correcta...
        if (other.CompareTag("ZonaSegura"))
        {
            yaSeRefugio = false;
            Debug.Log("⚠️ SALISTE DEL REFUGIO");
        }
    }

    IEnumerator SecuenciaRefugio()
    {
        Debug.Log("Te refugiaste. El sismo durará 5 segundos más...");
        
        // 1. Cuenta regresiva final
        yield return new WaitForSeconds(5.0f);
        
        // 2. Apagamos el movimiento físico y la vibración de la cámara
        scriptPrincipal.enZonaTerremoto = false;
        scriptPrincipal.FrenarSonido(); // (O simplemente FrenarSonido(); si estás adentro del mismo script)
        if (scriptCabeza != null) scriptCabeza.haySismo = false;
        
        Debug.Log("Sismo terminado. Iniciando apagón preventivo.");
        monitorOficina.EstablecerPaso(3);

        // 3. Cortamos la energía usando el array de tu SimuladorSismo
        foreach (GameObject luz in scriptPrincipal.lucesOficina)
        {
            if (luz != null) luz.SetActive(false);
        }
        scriptPrincipal.mochila.ActivarRescate();
        // --- NUEVO: EL APAGÓN TOTAL DE UNITY ---
        // Matamos la intensidad de la luz del cielo
        RenderSettings.ambientIntensity = 0f;
        // Apagamos los reflejos del entorno en los materiales
        RenderSettings.reflectionIntensity = 0f;
        Debug.Log("LOOOL");
        // Por si acaso, forzamos el color ambiental a negro puro
       // RenderSettings.ambientLight = Color.black;
       // --- NUEVO: ACTIVAR LA VÁLVULA ---
// --- PRENDER TODAS LAS VÁLVULAS ---
        foreach (ValvulaInteractiva valvula in valvulasDeEmergencia)
        {
            if (valvula != null)
            {
                valvula.IniciarAlertaEmergencia();
            }
        }
    }

}