using UnityEngine;

public class AgarrarObjeto : MonoBehaviour
{
    [Header("Ajustes de Agarre")]
    public float distanciaAlcance = 3f;
    public float potencia = 10.0f;
    public Transform puntoSujecion; 
    
    [Header("Referencia Visual")]
    public GameObject miraEsfera;
    private Renderer miraRenderer;
    private float offsetSuperficie = 0.05f; 

    // Variables internas
    private GameObject objetoAgarrado;
    private Rigidbody rbObjeto;
    private int capaOriginal; // Para recordar en qué capa estaba el objeto
    
    void Start()
    {
        if (miraEsfera != null)
            miraRenderer = miraEsfera.GetComponent<Renderer>();
    }

    void Update()
    {
        // === PARTE 1: SI NO TENEMOS NADA ===
        if(objetoAgarrado == null)
        {
            EscanearEntorno();
        }
        // === PARTE 2: SI TENEMOS ALGO AGARRADO ===
        else
        {
            MoverObjetoSinAtravesarParedes();
            
            // UI: Ponemos la mira blanca (o apagada)
            PintarMira(Color.white);
            ResetearPosicionMira();
        }

        // INPUT
        if (Input.GetButtonDown("Fire1")) 
        {
            if (objetoAgarrado == null) IntentarAgarrar();
            else Soltar();
        }
    }

    // --- LÓGICA DE MOVIMIENTO ---
    void MoverObjetoSinAtravesarParedes()
    {
        Vector3 destinoIdeal = puntoSujecion.position;
        RaycastHit hitObstaculo;

        if (Physics.Linecast(transform.position, destinoIdeal, out hitObstaculo))
        {
            objetoAgarrado.transform.position = hitObstaculo.point;
            objetoAgarrado.transform.rotation = puntoSujecion.rotation;
        }
        else
        {
            objetoAgarrado.transform.position = destinoIdeal;
            objetoAgarrado.transform.rotation = puntoSujecion.rotation;
        }
    }

    void IntentarAgarrar()
    {
        RaycastHit hit;
        int layerMask = ~LayerMask.GetMask("Ignore Raycast");

        if (Physics.Raycast(transform.position, transform.forward, out hit, distanciaAlcance, layerMask))
        {
            // --- NUEVA LÓGICA DE PUERTA ---
            if (hit.collider.TryGetComponent(out PuertaInteractiva puerta))
            {
                puerta.AlternarPuerta(); 
                return; 
            }

            // --- LÓGICA DE AGARRE (AHORA CON TAG) ---
            rbObjeto = hit.collider.GetComponent<Rigidbody>();
            
            // CONDICIÓN: Tiene que tener Rigidbody, NO ser la mira, Y tener el tag "Interactuable"
            if (rbObjeto != null && hit.collider.gameObject != miraEsfera && hit.collider.CompareTag("Interactuable")) 
            {
                objetoAgarrado = hit.collider.gameObject;
                capaOriginal = objetoAgarrado.layer;
                objetoAgarrado.layer = 2; // Lo pasa a Ignore Raycast
                rbObjeto.isKinematic = true; 
            }
        }
    }

    void Soltar()
    {
        if (rbObjeto != null)
        {
            // RESTAURAMOS TODO
            objetoAgarrado.layer = capaOriginal; 
            rbObjeto.isKinematic = false; 
            
            // Lanzamos con fuerza
            rbObjeto.AddForce(transform.forward * potencia, ForceMode.Impulse);
        }
        objetoAgarrado = null;
        rbObjeto = null;
    }

    void EscanearEntorno()
    {
        if (miraEsfera == null) return;

        RaycastHit hitScan;
        int layerMask = ~LayerMask.GetMask("Ignore Raycast");

        if (Physics.Raycast(transform.position, transform.forward, out hitScan, distanciaAlcance, layerMask))
        {
             miraEsfera.transform.position = hitScan.point + (hitScan.normal * offsetSuperficie);
             miraEsfera.transform.rotation = Quaternion.LookRotation(hitScan.normal);

             // CONDICIÓN VISUAL: Solo se pone verde si tiene Rigidbody Y tiene el tag "Interactuable"
             if (hitScan.collider.GetComponent<Rigidbody>() != null && hitScan.collider.CompareTag("Interactuable"))
                 PintarMira(Color.green);
             else
                 PintarMira(Color.white);
        }
        else
        {
             ResetearPosicionMira();
             PintarMira(Color.white);
        }
    }
    
    void ResetearPosicionMira()
    {
        if (miraEsfera == null) return;
        miraEsfera.transform.position = transform.position + (transform.forward * distanciaAlcance);
        miraEsfera.transform.LookAt(transform.position);
    }

    void PintarMira(Color color)
    {
        if (miraRenderer != null) miraRenderer.material.color = color;
    }
}