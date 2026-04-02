using UnityEngine;
using System.Collections;

public class TermicaInteractiva : MonoBehaviour
{
    private Renderer[] todosLosRenderers;
    private bool emergenciaActivada = false;
    private bool estaApagada = false;
    private Coroutine corrutinaTitileo;

    [Header("Configuración Visual")]
    [ColorUsage(true, true)]
    public Color colorAlerta = new Color(0f, 1f, 0f, 1f); // Verde

    void Start()
    {
        // Buscamos todos los componentes que pueden brillar en el modelo GLB
        todosLosRenderers = GetComponentsInChildren<Renderer>();
        ApagarBrillo();
    }

    public void IniciarAlertaEmergencia()
    {
        emergenciaActivada = true;
        if (todosLosRenderers.Length > 0)
        {
            corrutinaTitileo = StartCoroutine(TitilarEmission());
        }
    }

    // Esta es la función que debés llamar con tu sistema de interacción (Fire1)
    public void Interactuar()
    {
        if (emergenciaActivada && !estaApagada)
        {
            estaApagada = true;
            if (corrutinaTitileo != null) StopCoroutine(corrutinaTitileo);
            ApagarBrillo();

            // La línea que solicitaste para los puntos
            FindObjectOfType<GestorObjetivos>().MarcarObjetivo("Termica");
            Debug.Log("🔌 Térmica desactivada. Puntos sumados.");
        }
    }

    IEnumerator TitilarEmission()
    {
        foreach (Renderer ren in todosLosRenderers)
        {
            ren.material.EnableKeyword("_EMISSION");
        }

        while (!estaApagada)
        {
            float intensidad = (Mathf.Sin(Time.time * 4f) + 1f) / 2f;
            Color colorActual = colorAlerta * (intensidad * 2f);

            foreach (Renderer ren in todosLosRenderers)
            {
                ren.material.SetColor("_EmissionColor", colorActual);
            }
            yield return null;
        }
    }

    void ApagarBrillo()
    {
        foreach (Renderer ren in todosLosRenderers)
        {
            ren.material.SetColor("_EmissionColor", Color.black);
            ren.material.DisableKeyword("_EMISSION");
        }
    }
}