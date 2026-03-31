using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    // Nombre de tu escena de sismo (asegúrate de que coincida exacto)
    public string nombreEscenaJuego = "oficina";

    public void IniciarJuego()
    {
        Debug.Log("Intentando cargar la escena: " + nombreEscenaJuego);
        
        // Verificamos si la escena existe en los Build Settings antes de cargar
        if (Application.CanStreamedLevelBeLoaded(nombreEscenaJuego))
        {
            SceneManager.LoadScene(nombreEscenaJuego);
        }
        else
        {
            Debug.LogError("ERROR: La escena '" + nombreEscenaJuego + "' no se encuentra en Build Settings o el nombre está mal escrito.");
        }
    }

    public void CerrarApp()
    {
        Debug.Log("Cerrando aplicación...");
        Application.Quit();
        
        // Si estás probando en el Editor, esto ayuda a ver que funciona
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}
