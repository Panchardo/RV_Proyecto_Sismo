using UnityEngine;

public class CalibradorVR : MonoBehaviour
{
    [Header("Componentes de Cámara VR")]
    // ATENCIÓN: Ahora son tipo 'Camera', no 'Transform'
    public Camera ojoIzquierdo;
    public Camera ojoDerecho;

    [Header("Configuración IPD (Eje Horizontal)")]
    public float ipdActual = 0.064f; 
    public float ipdMinimo = 0.050f;
    public float ipdMaximo = 0.080f;
    public float velocidadIPD = 0.02f; 
    public string botonIPD = "Jump"; // El botón que ya usabas

    [Header("Configuración FOV (Eje Vertical)")]
    public float fovActual = 80f;
    public float fovMinimo = 60f;
    public float fovMaximo = 110f;
    public float velocidadFOV = 15f; // Grados por segundo
    public string botonFOV = "Fire1"; // Acá poné el nombre de tu botón Cuadrado (Agarre)

    private bool ajustandoIPD = false;
    private bool ajustandoFOV = false;

    void Start()
    {
        AplicarIPD();
        AplicarFOV();
    }

    void Update()
    {
        // Leemos el estado de los "embragues" lógicos
        ajustandoIPD = Input.GetButton(botonIPD);
        ajustandoFOV = Input.GetButton(botonFOV);

        // --- LÓGICA DE IPD (Izquierda / Derecha) ---
        if (ajustandoIPD)
        {
            float inputX = Input.GetAxis("Horizontal"); 
            if (Mathf.Abs(inputX) > 0.05f)
            {
                ipdActual += inputX * velocidadIPD * Time.deltaTime;
                ipdActual = Mathf.Clamp(ipdActual, ipdMinimo, ipdMaximo);
                AplicarIPD();
            }
        }

        // --- LÓGICA DE FOV (Arriba / Abajo) ---
        if (ajustandoFOV)
        {
            // Usamos el eje vertical del joystick para "acercar/alejar" el FOV
            float inputY = Input.GetAxis("Vertical"); 
            if (Mathf.Abs(inputY) > 0.05f)
            {
                // Sumamos los grados. Arriba aumenta el FOV (aleja), Abajo lo achica (acerca).
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
            // Modificamos el Transform asociado a la cámara
            ojoIzquierdo.transform.localPosition = new Vector3(-ipdActual / 2f, 0f, 0f);
            ojoDerecho.transform.localPosition = new Vector3(ipdActual / 2f, 0f, 0f);
        }
    }

    void AplicarFOV()
    {
        if (ojoIzquierdo != null && ojoDerecho != null)
        {
            // Modificamos la óptica virtual
            ojoIzquierdo.fieldOfView = fovActual;
            ojoDerecho.fieldOfView = fovActual;
        }
    }

    void OnGUI()
    {
        // Solo mostramos la UI si estamos apretando alguno de los dos botones
        if (ajustandoIPD || ajustandoFOV)
        {
            GUIStyle estilo = new GUIStyle(GUI.skin.label);
            estilo.fontSize = 45;
            estilo.normal.textColor = Color.yellow;
            estilo.alignment = TextAnchor.MiddleCenter;
            
            string textoMostrar = "";
            
            // Definimos qué texto mostrar según qué botón estés apretando
            if (ajustandoIPD) textoMostrar = "Ajuste IPD:\n" + (ipdActual * 1000f).ToString("F0") + " mm";
            if (ajustandoFOV) textoMostrar = "Ajuste FOV:\n" + fovActual.ToString("F1") + "°";

            // Ojo Izquierdo
            Rect rectIzquierdo = new Rect(0, 100, Screen.width / 2, 150);
            GUI.Label(rectIzquierdo, textoMostrar, estilo);

            // Ojo Derecho
            Rect rectDerecho = new Rect(Screen.width / 2, 100, Screen.width / 2, 150);
            GUI.Label(rectDerecho, textoMostrar, estilo);
        }
    }
}