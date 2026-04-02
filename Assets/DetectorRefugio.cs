using UnityEngine;
using System.Collections;

public class DetectorRefugio : MonoBehaviour
{
    [Header("Referencias")]
    public ValvulaInteractiva[] valvulasDeEmergencia;
    public SimuladorSismo scriptPrincipal;
    public ControlCabeza scriptCabeza;
    public bool yaSeRefugio = false; 
    public MochilaEmergencia mochila;
    public PanelDialogo monitorOficina;
    public TermicaInteractiva scriptTermica;
    private bool puntosEntregados = false; // Evita que sume puntos dos veces

    void OnTriggerEnter(Collider other)
    {
        if (mochila.getMochilardaLista())
        {
            if (other.CompareTag("ZonaSegura"))
            {
                yaSeRefugio = true;

                if (scriptPrincipal.enZonaTerremoto)
                {
                    // --- ARREGLO: APAGAMOS LOS CUBOS VERDES ACÁ ---
                    scriptPrincipal.ApagarResaltados();

                    // --- NUEVO: LEER QUÉ REFUGIO ES Y DAR PUNTOS ---
                    InfoRefugio info = other.GetComponent<InfoRefugio>();
                    if (info != null && !puntosEntregados)
                    {
                        FindObjectOfType<GestorObjetivos>().MarcarObjetivo(info.idObjetivo);
                        puntosEntregados = true;
                    }
                    // ------------------------------------------------
                    
                    StartCoroutine(SecuenciaRefugio());  
                }
                
                Debug.Log("✅ ¡ESTAS A SALVO!");
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("ZonaSegura"))
        {
            yaSeRefugio = false;
            Debug.Log("⚠️ SALISTE DEL REFUGIO");
        }
    }

    IEnumerator SecuenciaRefugio()
    {
        Debug.Log("Te refugiaste. El sismo durará 5 segundos más...");
        yield return new WaitForSeconds(5.0f);
        
        scriptPrincipal.enZonaTerremoto = false;
        scriptPrincipal.FrenarSonido(); 
        if (scriptCabeza != null) scriptCabeza.haySismo = false;
        
        Debug.Log("Sismo terminado. Iniciando apagón preventivo.");
        monitorOficina.EstablecerPaso(3);

        foreach (GameObject luz in scriptPrincipal.lucesOficina)
        {
            if (luz != null) luz.SetActive(false);
        }

        scriptPrincipal.mochila.ActivarRescate();

        RenderSettings.ambientIntensity = 0f;
        RenderSettings.reflectionIntensity = 0f;

        foreach (ValvulaInteractiva valvula in valvulasDeEmergencia)
        {
            if (valvula != null)
            {
                valvula.IniciarAlertaEmergencia();
            }
        }
                // --- NUEVO: PRENDER TÉRMICA ---
        if (scriptTermica != null) scriptTermica.IniciarAlertaEmergencia();
        // Programamos una réplica para dentro de 15 segundos (cambiá el número a tu gusto)
        scriptPrincipal.ProgramarReplica(10.0f);
    }
}