using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class GestorObjetivos : MonoBehaviour
{
    public TextMeshProUGUI textoListaUI;

    [System.Serializable]
    public class Objetivo {
        public string id; // Nombre interno (ej: "Gas")
        public string nombrePantalla; // Lo que lee el usuario
        public int puntos;
        public bool completado = false;
    }

    public List<Objetivo> objetivos = new List<Objetivo>();
    private GestorJuego gestor;

    void Start() {
        gestor = GetComponent<GestorJuego>();
        ActualizarUI();
    }

    public void MarcarObjetivo(string id) {
        foreach (var obj in objetivos) {
            if (obj.id == id && !obj.completado) {
                obj.completado = true;
                gestor.SumarPuntos(obj.puntos);
                ActualizarUI();
                break;
            }
        }
    }

    void ActualizarUI() {
        string lista = "<color=#00FFFF>POSIBLES:</color>\n";
        foreach (var obj in objetivos) {
            if (obj.completado) lista += $"<s><color=green>- {obj.nombrePantalla}</color></s>\n";
            else lista += $"- {obj.nombrePantalla}\n";
        }
        textoListaUI.text = lista;
    }
}