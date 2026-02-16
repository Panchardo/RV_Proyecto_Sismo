using UnityEngine;

public class TableroElectrico : MonoBehaviour
{
    [Header("Componentes Visuales")]
    public Transform palanca; // Arrastrá acá el cubito de la palanca
    public float anguloRotacion = 50f; // Cuánto baja la térmica

    [Header("Sistema Eléctrico")]
    public Light[] lucesOficina; // Arrastrá todas las luces de tu oficina acá
    
    [Header("Gestión de Escena")]
    //public GestorDeMundos gestorEscenarios; // El script que armamos antes

    private bool yaSeCorto = false;

    // Esta función la llamaremos desde tu script de Agarrar
    public void AccionarTermica()
    {
        if (yaSeCorto) return;

        // 1. Efecto Mecánico: Rotamos la palanca hacia abajo
        palanca.localRotation = Quaternion.Euler(anguloRotacion, 0, 0);

        // 2. Efecto Eléctrico: Apagamos el array de luces
        foreach (Light luz in lucesOficina)
        {
            if (luz != null) luz.enabled = false;
        }

        yaSeCorto = true;
        Debug.Log("Suministro eléctrico cortado. Protocolo de evacuación iniciado.");

        // 3. (Opcional) Avisamos al gestor que ya podemos salir
        // Si querés que el cambio de mundo sea automático al cortar la luz:
        // if (gestorEscenarios != null) gestorEscenarios.ActivarEvacuacion();
    }
}