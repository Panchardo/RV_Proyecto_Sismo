using UnityEngine;
using System.Collections.Generic;

public class ItemEmergencia : MonoBehaviour
{
    public MochilaEmergencia mochilaDestino; 
    public string nombreItem;

    // Usamos una lista para guardar absolutamente todos los materiales de la pieza
    private List<Material> materiales = new List<Material>();

    void Start()
    {
        // 1. Buscamos TODOS los renderizadores en el objeto y en sus objetos "hijos"
        Renderer[] renderizadores = GetComponentsInChildren<Renderer>();
        
        // 2. Recorremos cada renderizador encontrado
        foreach (Renderer rend in renderizadores)
        {
            // 3. Un renderizador puede tener varios materiales, así que también los recorremos
            foreach (Material mat in rend.materials)
            {
                mat.EnableKeyword("_EMISSION");
                materiales.Add(mat); // Lo guardamos en nuestra lista maestra
            }
        }
    }

    void Update()
    {
        // Efecto "Respiración LED"
        float pulso = Mathf.PingPong(Time.time * 1.5f, 0.5f);
        Color colorBrillo = Color.yellow * pulso;

        // Le mandamos el pulso de luz a todos los materiales al mismo tiempo
        foreach (Material mat in materiales)
        {
            mat.SetColor("_EmissionColor", colorBrillo);
        }
    }

    public void Recolectar()
    {
        if (mochilaDestino != null)
        {
            mochilaDestino.RegistrarItem(nombreItem);
        }
        else
        {
            Debug.LogWarning("Ojo: No le asignaste la mochila destino a " + gameObject.name);
        }
        
        gameObject.SetActive(false); 
    }
}