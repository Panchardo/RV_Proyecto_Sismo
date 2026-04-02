using UnityEngine;


public class MochilaEmergencia : MonoBehaviour
{
    private int itemsRecolectados = 0;
    private int totalItems = 5; // Linterna, Radio, Botiquín
    public bool mochilaLista = false;
    public PanelDialogo monitorOficina;
    private Material mat;
    private bool activarResaltado = false;

    [Header("Fase de Apagón")]
    public bool enModoRescate = false;
    public GameObject modeloLinternaEnMano;

    public DetectorTrigger cubiculo;

    void Start()
    {
        mat = GetComponent<Renderer>().material;
        mat.EnableKeyword("_EMISSION");
        mat.SetColor("_EmissionColor", Color.black); // Arranca apagada
    }

    void Update()
    {
        if (activarResaltado)
        {
            // Pulso verde indicando que está lista para recibir los items
            float pulso = Mathf.PingPong(Time.time * 2f, 0.8f);
            Color colorBrillo = enModoRescate ? Color.yellow : Color.green;
            mat.SetColor("_EmissionColor", colorBrillo * pulso);
        }
    }

    public void RegistrarItem(string nombre)
    {
        itemsRecolectados++;
        Debug.Log("Recogiste: " + nombre + " (" + itemsRecolectados + "/" + totalItems + ")");

        if (itemsRecolectados >= totalItems)
        {
            
            activarResaltado = true;
            Debug.Log("¡Items listos! Interactuá con la mochila para guardarlos.");
        }
    }

    public void InteractuarMochila()
    {
        // FASE 1: Guardar las cosas antes del sismo
        if (itemsRecolectados >= totalItems && !mochilaLista && !enModoRescate)
        {
            monitorOficina.EstablecerPaso(1);
            mochilaLista = true;
            activarResaltado = false;
            mat.SetColor("_EmissionColor", Color.black); 
            FindObjectOfType<GestorObjetivos>().MarcarObjetivo("Mochila");
            Debug.Log("Mochila equipada. Ve a tu cubículo.");
            cubiculo.renderizadorZona.enabled = true;
            cubiculo.ActualizarColorZona();
        }
        // FASE 2: Agarrar la linterna durante el apagón
        else if (enModoRescate)
        {
            enModoRescate = false; // Se apaga la mochila
            activarResaltado = false;
            mat.SetColor("_EmissionColor", Color.black);
            gameObject.SetActive(false);

            // Aparece la linterna en tu mano y, como su luz interna ya está en ON, ilumina al instante.
            if (modeloLinternaEnMano != null) 
            {
                modeloLinternaEnMano.SetActive(true);
            }

            Debug.Log("Linterna encendida. ¡Busca la salida de emergencia!");
        }
    }

    public bool getMochilardaLista(){
        
        return mochilaLista;
    }

    public void ActivarRescate()
    {
        enModoRescate = true;
        activarResaltado = true;
    }

}