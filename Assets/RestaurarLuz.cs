using UnityEngine;

public class RestaurarLuz : MonoBehaviour
{
    [Header("Iluminación Exterior")]
    public GameObject solDirectionalLight; // Arrastrá tu "Directional Light" acá

    private bool yaSalio = false;

    void OnTriggerEnter(Collider other)
    {
        // Solo reacciona si es el jugador y si no se activó antes
        if (other.CompareTag("Player") && !yaSalio)
        {
            yaSalio = true;
            Debug.Log("Saliendo de la planta. Restaurando iluminación global.");

            // 1. Restauramos la luz ambiental de Unity (el resplandor del cielo)
            RenderSettings.ambientIntensity = 1f; 
            RenderSettings.reflectionIntensity = 1f;

            // 2. Volvemos a prender el foco principal del Sol
            if (solDirectionalLight != null)
            {
                solDirectionalLight.SetActive(true);
            }
            
            // Opcional: Podrías hacer que la linterna de la mano se apague sola acá 
             other.GetComponentInChildren<Light>().enabled = false;
        }
    }
}