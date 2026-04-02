using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GestorJuego : MonoBehaviour
{
    [Header("Estadísticas")]
    public int puntajeActual = 0;
    public float tiempoJugado = 0f;
    private bool juegoTerminado = false;

    [Header("UI Final (Fade)")]
    public CanvasGroup grupoPantallaNegra; 
    public TextMeshProUGUI textoFinal;
    public float velocidadFade = 1f;

    void Update() {
        if (!juegoTerminado) {
            tiempoJugado += Time.deltaTime;
        } else {
            if (grupoPantallaNegra.alpha < 1f) grupoPantallaNegra.alpha += Time.deltaTime * velocidadFade;
            if (Input.GetButtonDown("Fire1")) SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }

    public void SumarPuntos(int puntos) {
        puntajeActual += puntos;
    }

    public void FinalizarSimulacion() {
        juegoTerminado = true;
        int min = Mathf.FloorToInt(tiempoJugado / 60f);
        int seg = Mathf.FloorToInt(tiempoJugado % 60f);
        
        textoFinal.text = $"<b>SIMULACIÓN ARGOS FINALIZADA</b>\n\n" +
                          $"Puntaje de Seguridad: <color=yellow>{puntajeActual}/100</color>\n" +
                          $"Tiempo de Evacuación: {min:00}:{seg:00}\n\n" +
                          $"<i>Presiona el botón para reiniciar</i>";
    }
}