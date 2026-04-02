using UnityEngine;

public class ZonaPuntaje : MonoBehaviour
{
    public string idObjetivo; // Debe coincidir con el ID en la lista
    public bool esZonaFinal = false;
    private bool yaActivo = false;

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player") && !yaActivo) {
            yaActivo = true;
            FindObjectOfType<GestorObjetivos>().MarcarObjetivo(idObjetivo);
            if (esZonaFinal) FindObjectOfType<GestorJuego>().FinalizarSimulacion();
        }
    }
}