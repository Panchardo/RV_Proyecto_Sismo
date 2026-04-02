using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDirector : MonoBehaviour
{
    [Header("Referencias")]
    public GestorJuego gestor; // <-- Tu arreglo, acoplamos el gestor directamente

    [Header("UI de Fin de Simulación")]
    public GameObject pantallaNegra; 
    public TextMeshProUGUI textoEstadisticas;
    public string botonInteractuar = "Fire1";

    private float tiempoJugado = 0f;
    
    // --- NUEVO: SISTEMA DE FASES ---
    // 0 = Jugando | 1 = Pantalla Puntaje | 2 = Pantalla Detalles
    private int faseFinal = 0; 

    void Start()
    {
        if (pantallaNegra != null) pantallaNegra.SetActive(false);
    }

    void Update()
    {
        if (faseFinal == 0) // JUGANDO
        {
            tiempoJugado += Time.deltaTime;
        }
        else if (faseFinal == 1) // PANTALLA 1: RESUMEN BÁSICO
        {
            if (Input.GetButtonDown(botonInteractuar))
            {
                MostrarPantallaDetalles();
            }
        }
        else if (faseFinal == 2) // PANTALLA 2: EL "TICKET" Y REINICIO
        {
            if (Input.GetButtonDown(botonInteractuar))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void TerminarSimulacion()
    {
        if (faseFinal > 0) return; // Evita que se llame dos veces

        faseFinal = 1; // Pasamos a la Fase 1

        int minutos = Mathf.FloorToInt(tiempoJugado / 60F);
        int segundos = Mathf.FloorToInt(tiempoJugado - minutos * 60);
        string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

        if (pantallaNegra != null) pantallaNegra.SetActive(true);

        if (textoEstadisticas != null)
        {
            // PANTALLA 1: Llamamos a gestor.puntajeActual tal cual lo arreglaste vos
            textoEstadisticas.text = "<b><color=#00FFFF>SIMULACION COMPLETADA</color></b>\n\n" +
                                     "Puntaje Final: <color=yellow>" + gestor.puntajeActual + " / 100</color>\n" +
                                     "Tiempo total: <color=white>" + tiempoFormateado + "</color>\n\n" +
                                     "<i>Presiona el botón de acción para ver el reporte detallado</i>";
        }
    }

    // --- NUEVA FUNCIÓN PARA LA SEGUNDA PANTALLA ---
    private void MostrarPantallaDetalles()
    {
        faseFinal = 2; // Pasamos a la Fase 2

        string reporte = "<b><color=#00FFFF>REPORTE DEL PROTOCOLO</color></b>\n\n<size=80%>";
        
        GestorObjetivos gestorObj = FindObjectOfType<GestorObjetivos>();
        
        if (gestorObj != null)
        {
            foreach (var obj in gestorObj.objetivos)
            {
                if (obj.completado) 
                {
                    // Cambiamos el símbolo raro por un [OK]
                    reporte += $"<color=#55FF55>[OK] {obj.nombrePantalla} (+{obj.puntos} pts)</color>\n";
                } 
                else 
                {
                    // Cambiamos la cruz rara por una [X]
                    reporte += $"<color=#FF5555>[ X ] {obj.nombrePantalla} (Fallo/Ignorado)</color>\n";
                }
            }
        }
        reporte += "</size>\n\n<i>Presiona el botón de acción para reiniciar el nivel</i>";

        if (textoEstadisticas != null)
        {
            // Reemplazamos el texto viejo por el reporte nuevo
            textoEstadisticas.text = reporte;
        }
    }
}