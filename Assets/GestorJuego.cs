using UnityEngine;
using TMPro;

public class GestorJuego : MonoBehaviour
{
    public int puntajeActual = 0;
    public bool sismoActivo = false; // Lo prendés desde tu SimuladorSismo

    public void SumarPuntos(int puntos)
    {
        puntajeActual += puntos;
        // Podés meter un Debug.Log acá para ver en la consola si suma bien
        Debug.Log("¡Puntos sumados! Total: " + puntajeActual);
    }
}