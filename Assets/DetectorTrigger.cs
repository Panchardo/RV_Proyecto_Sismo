using UnityEngine;
using System.Collections;

public class DetectorTrigger : MonoBehaviour
{
    public SimuladorSismo scriptPrincipal; 
    public ControlCabeza scriptCabeza;
    
    // --- NUEVO: REFERENCIA A LA TELE ---
    public PanelDialogo monitorOficina; 

    private bool yaSeUso = false;
    public Color colorLibre = new Color(0f, 1f, 0f, 0.3f);   // Verde
    public MeshRenderer renderizadorZona; 

    void OnTriggerStay(Collider other)
    {
        // 1. Verificamos que sea el jugador y que no se haya usado antes
        if (other.CompareTag("Player") && !yaSeUso)
        {
            // 2. Le preguntamos a la mochila si está lista
            if (scriptPrincipal.mochila.getMochilardaLista())
            {
                yaSeUso = true; // Se quema este trigger para no volver a llamarse
                ApagarEfectoVisual();
                
                // ACTIVACIÓN INFINITA: Prendemos el sismo y lo dejamos así
                scriptPrincipal.enZonaTerremoto = true;
                if (scriptCabeza != null) scriptCabeza.haySismo = true;
                
                Debug.Log("¡Sismo infinito iniciado! Buscá refugio bajo el escritorio.");
            
                
                // --- NUEVO: PRENDEMOS EL SONIDO ---
                scriptPrincipal.IniciarSonido();
                

                // --- NUEVO: LE AVISAMOS A LA TELE QUE CAMBIE AL TEXTO DE CUBRIRSE (Elemento 2) ---
                if (monitorOficina != null) 
                {
                    monitorOficina.EstablecerPaso(2);
                }
            }
        }
    }

    public void ActualizarColorZona()
    {
        if (renderizadorZona != null)
        {
            // Cambiamos el color del material dinámicamente
            renderizadorZona.material.color = colorLibre;
        }
    }

    private void ApagarEfectoVisual()
    {
        if (renderizadorZona != null)
        {
            renderizadorZona.enabled = false; // Oculta el cubo por completo
        }
    }
}