using UnityEngine;

public class LimitarFPS : MonoBehaviour
{
    void Awake()
    {
        // Limitamos a 30 cuadros para que el USB no explote
        Application.targetFrameRate = 30;
    }
}