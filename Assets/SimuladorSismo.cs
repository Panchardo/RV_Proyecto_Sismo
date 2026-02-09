using UnityEngine;

public class SimuladorSismo : MonoBehaviour
{
    public float magnitudSismo = 20f; // Fuerza del empujón (Subile si no se mueven)
    private Rigidbody[] objetosAfectados; 

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        // 1. Llenamos el array automáticamente al iniciar el juego
        // FindObjectsOfType busca CADA objeto en la escena que tenga Rigidbody
        objetosAfectados = FindObjectsOfType<Rigidbody>();
        
        // Debug para que veas en consola cuántos encontró
        Debug.Log("Se encontraron " + objetosAfectados.Length + " objetos físicos.");
    }

    void Update()
    {
        // Mantené apretada la T para el sismo
        if (Input.GetKey(KeyCode.T))
        {
            Temblar();
        }
    }

    void Temblar()
    {
        // 2. Recorremos la lista uno por uno
        foreach (Rigidbody rb in objetosAfectados)
        {
             // Si el objeto es "Kinematic" (como quizás tu mano), no lo empujamos
             if(rb.isKinematic) continue;

             // 3. Vector random (X, Y, Z) entre -1 y 1
             Vector3 direccionRandom = Random.insideUnitSphere;
             
             // 4. Aplanamos la fuerza (Sismo horizontal)
             direccionRandom.y = 0; 
             // Normalizamos para que la dirección mida 1, y multiplicamos por magnitud
             Vector3 fuerzaFinal = direccionRandom.normalized * magnitudSismo;

             // 5. Aplicamos la fuerza. 
             // ForceMode.Force es continuo (como viento o empuje constante)
             // ForceMode.Impulse es golpes (como martillazos). Probá ambos.
             rb.AddForce(fuerzaFinal, ForceMode.Impulse);
        }
    }
}