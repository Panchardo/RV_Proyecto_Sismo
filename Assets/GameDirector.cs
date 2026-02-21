using UnityEngine;
using UnityEngine.SceneManagement; // Necesario para reiniciar el nivel
using TMPro;

public class GameDirector : MonoBehaviour
{
    [Header("UI de Fin de Simulación")]
    public GameObject pantallaNegra; // El Canvas que tapa la vista
    public TextMeshProUGUI textoEstadisticas;

    [Header("Controles")]
    public string botonInteractuar = "Fire1";

    private float tiempoJugado = 0f;
    private bool juegoTerminado = false;

    void Start()
    {
        // Nos aseguramos de que la pantalla negra empiece apagada
        if (pantallaNegra != null) pantallaNegra.SetActive(false);
    }

    void Update()
    {
        // Mientras no llegues a la zona segura, el reloj sigue corriendo
        if (!juegoTerminado)
        {
            tiempoJugado += Time.deltaTime;
        }
        else
        {
            // Si ya terminó y apretás el botón de tu ESP32/Joystick, se reinicia todo
            if (Input.GetButtonDown(botonInteractuar))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }

    public void TerminarSimulacion()
    {
        if (juegoTerminado) return; 

        juegoTerminado = true;

        // Matemática simple para convertir los segundos en "Minutos:Segundos"
        int minutos = Mathf.FloorToInt(tiempoJugado / 60F);
        int segundos = Mathf.FloorToInt(tiempoJugado - minutos * 60);
        string tiempoFormateado = string.Format("{0:00}:{1:00}", minutos, segundos);

        // Prendemos la pantalla negra
        if (pantallaNegra != null) pantallaNegra.SetActive(true);

        // Escribimos el resultado
        if (textoEstadisticas != null)
        {
            textoEstadisticas.text = "<b><color=#00FFFF>SIMULACION COMPLETADA</color></b>\n\n" +
                                     "Evacuación exitosa a Zona Segura.\n\n" +
                                     "Tiempo total: <color=yellow>" + tiempoFormateado + "</color>\n\n" +
                                     "<i>Presiona el botón para reiniciar simulación</i>";
        }
    }
}