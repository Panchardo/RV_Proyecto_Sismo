using UnityEngine;

public class MapeoDeBotones : MonoBehaviour
{
    private string ultimoBoton = "Apretá un botón...";
    private float tiempoVisible = 0f;

    void Update()
    {
        // Unity soporta hasta 20 botones genéricos por joystick
        // Iteramos por todos para ver cuál se activó en este frame
        for (int i = 0; i < 20; i++)
        {
            // La sintaxis interna de Unity para esto es "joystick button X"
            if (Input.GetKeyDown("joystick button " + i))
            {
                ultimoBoton = "Botón: " + i;
                tiempoVisible = 3f; // Lo mantenemos en pantalla 3 segundos
                
                // También lo mandamos a la consola por si estás en la PC
                Debug.Log("Se detectó el " + ultimoBoton);
            }
        }

        // Un temporizador simple para limpiar la pantalla
        if (tiempoVisible > 0)
        {
            tiempoVisible -= Time.deltaTime;
        }
        else
        {
            ultimoBoton = "Esperando input...";
        }
    }

    void OnGUI()
    {
        // Hacemos la letra bien grande y verde fosforescente para que 
        // resalte por encima de la oficina o la calle
        GUI.color = Color.green;
        GUI.skin.label.fontSize = 60;
        
        // Lo ubicamos en la parte inferior central de la pantalla
        GUILayout.BeginArea(new Rect(Screen.width / 2 - 250, Screen.height - 150, 500, 100));
        GUILayout.Label(ultimoBoton);
        GUILayout.EndArea();
    }
}