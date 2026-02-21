using UnityEngine;

public class CalibradorVR : MonoBehaviour
{
    [Header("Componentes de Cámara VR")]
    public Camera ojoIzquierdo;
    public Camera ojoDerecho;

    [Header("Configuración IPD (Eje Horizontal)")]
    public float ipdActual = 0.064f; 
    public float ipdMinimo = 0.050f;
    public float ipdMaximo = 0.080f;
    public float velocidadIPD = 0.02f; 

    [Header("Configuración FOV (Eje Vertical)")]
    public float fovActual = 80f;
    public float fovMinimo = 60f;
    public float fovMaximo = 110f;
    public float velocidadFOV = 15f;

    [Header("Controles y Sensibilidad")]
    public string botonInteractuar = "Fire1"; 
    public float tiempoParaActivar = 2.0f;    
    
    // --- NUEVA VARIABLE PARA LA ZONA MUERTA ---
    [Range(0.1f, 0.9f)]
    public float zonaMuertaAnalogico = 0.4f; 

    private float tiempoApretado = 0f;
    public bool enModoCalibracion = false; 

    void Start()
    {
        AplicarIPD();
        AplicarFOV();
    }

    void Update()
    {
        // 1. LÓGICA DEL TEMPORIZADOR
        if (Input.GetButton(botonInteractuar))
        {
            tiempoApretado += Time.deltaTime; 
            if (tiempoApretado >= tiempoParaActivar)
            {
                enModoCalibracion = true;
            }
        }
        else
        {
            tiempoApretado = 0f;
            enModoCalibracion = false;
        }

        // 2. LÓGICA DE CALIBRACIÓN CON ZONA MUERTA INTELIGENTE
        if (enModoCalibracion)
        {
            float inputX = Input.GetAxis("Horizontal"); 
            float inputY = Input.GetAxis("Vertical"); 

            // Calculamos la fuerza absoluta de cada movimiento (sin importar si es negativo o positivo)
            float fuerzaX = Mathf.Abs(inputX);
            float fuerzaY = Mathf.Abs(inputY);

            // GANA EL EJE X (IPD): Si pasaste la zona muerta Y el movimiento horizontal es mayor al vertical
            if (fuerzaX > zonaMuertaAnalogico && fuerzaX > fuerzaY)
            {
                ipdActual += inputX * velocidadIPD * Time.deltaTime;
                ipdActual = Mathf.Clamp(ipdActual, ipdMinimo, ipdMaximo);
                AplicarIPD();
            }
            // GANA EL EJE Y (FOV): Si pasaste la zona muerta Y el movimiento vertical es mayor al horizontal
            else if (fuerzaY > zonaMuertaAnalogico && fuerzaY > fuerzaX)
            {
                fovActual += inputY * velocidadFOV * Time.deltaTime;
                fovActual = Mathf.Clamp(fovActual, fovMinimo, fovMaximo);
                AplicarFOV();
            }
        }
    }

    void AplicarIPD()
    {
        if (ojoIzquierdo != null && ojoDerecho != null)
        {
            ojoIzquierdo.transform.localPosition = new Vector3(-ipdActual / 2f, 0f, 0f);
            ojoDerecho.transform.localPosition = new Vector3(ipdActual / 2f, 0f, 0f);
        }
    }

    void AplicarFOV()
    {
        if (ojoIzquierdo != null && ojoDerecho != null)
        {
            ojoIzquierdo.fieldOfView = fovActual;
            ojoDerecho.fieldOfView = fovActual;
        }
    }

    void OnGUI()
    {
        if (enModoCalibracion)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.label);
            estilo.fontSize = 40;
            estilo.normal.textColor = Color.yellow;
            estilo.alignment = TextAnchor.MiddleCenter;
            
            string textoMostrar = "MODO CALIBRACIÓN\n" + 
                                  "IPD: " + (ipdActual * 1000f).ToString("F0") + " mm\n" + 
                                  "FOV: " + fovActual.ToString("F1") + "°";

            Rect rectIzquierdo = new Rect(0, Screen.height / 2 - 100, Screen.width / 2, 200);
            GUI.Label(rectIzquierdo, textoMostrar, estilo);

            Rect rectDerecho = new Rect(Screen.width / 2, Screen.height / 2 - 100, Screen.width / 2, 200);
            GUI.Label(rectDerecho, textoMostrar, estilo);
        }
        else if (tiempoApretado > 0.5f) 
        {
            GUIStyle estiloCarga = new GUIStyle(GUI.skin.label);
            estiloCarga.fontSize = 25;
            estiloCarga.normal.textColor = Color.white;
            estiloCarga.alignment = TextAnchor.LowerCenter;
            
            Rect rectCargaIzq = new Rect(0, Screen.height - 100, Screen.width / 2, 50);
            Rect rectCargaDer = new Rect(Screen.width / 2, Screen.height - 100, Screen.width / 2, 50);
            
            GUI.Label(rectCargaIzq, "Mantén para calibrar...", estiloCarga);
            GUI.Label(rectCargaDer, "Mantén para calibrar...", estiloCarga);
        }
    }
}