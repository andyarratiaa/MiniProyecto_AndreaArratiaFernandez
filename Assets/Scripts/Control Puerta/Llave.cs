using UnityEngine;

public class Llave : MonoBehaviour
{
    public ControlPuerta puerta1; // Primera puerta que abre esta llave
    public ControlPuerta puerta2; // Segunda puerta que abre esta llave

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            if (puerta1 != null && puerta2 != null)
            {
                puerta1.ObtenerLlave(); // Da acceso a la primera puerta
                puerta2.ObtenerLlave(); // Da acceso a la segunda puerta
            }

            Debug.Log("Has recogido la llave para estas dos puertas.");
            Destroy(gameObject); // La llave desaparece tras recogerla
        }
    }
}




