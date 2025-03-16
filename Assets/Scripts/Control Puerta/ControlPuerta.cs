//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ControlPuerta : MonoBehaviour
//{
//    Animator anim;

//    public bool Dentro = false;
//    bool puerta = false;

//    // Use this for initialization
//    void Start()
//    {
//        anim = GetComponent<Animator>();
//    }

//    void OnTriggerEnter(Collider col)
//    {
//        if (col.tag == "Player")
//        {
//            Dentro = true;
//        }
//    }

//    void OnTriggerExit(Collider col)
//    {
//        if (col.tag == "Player")
//        {
//            Dentro = false;
//        }
//    }

//    // Update is called once per frame
//    void Update() 
//    {
//        if(Dentro && Input.GetKeyDown(KeyCode.E))
//        {
//            puerta = !puerta;
//        }

//        if (puerta)
//        {
//            anim.SetBool("Abierto", true);
//        }
//        else
//        {
//            anim.SetBool("Abierto", false);
//        }
//    }

//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ControlPuerta : MonoBehaviour
{
    Animator anim;
    public bool Dentro = false;
    bool puerta = false;
    public bool tieneLlave = false; // La puerta solo se abre si tiene la llave
    public string nombreEscenaDestino; // Nombre de la escena a la que se cambiará
    public Transform posicionLlegada; // Posición donde aparecerá el jugador en la siguiente escena

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Dentro = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.CompareTag("Player"))
        {
            Dentro = false;
        }
    }

    void Update()
    {
        if (Dentro && Input.GetKeyDown(KeyCode.E))
        {
            if (tieneLlave) // Si el jugador tiene la llave correcta
            {
                puerta = !puerta;
                anim.SetBool("Abierto", puerta);

                if (puerta) // Si la puerta se abre, cambiar de escena
                {
                    StartCoroutine(CambiarEscena());
                }
            }
            else
            {
                Debug.Log("Necesitas la llave para abrir esta puerta.");
            }
        }
    }

    // Método para asignar la llave a esta puerta (llamado desde el script de la llave)
    public void ObtenerLlave()
    {
        tieneLlave = true;
        Debug.Log("Has obtenido la llave para estas puertas.");
    }

    // Método para cambiar de escena con una pequeña espera
    IEnumerator CambiarEscena()
    {
        yield return new WaitForSeconds(1f); // Pequeña pausa antes de cambiar de escena

        // Guardar la posición de llegada en PlayerPrefs (para que se use en la nueva escena)
        if (posicionLlegada != null)
        {
            PlayerPrefs.SetFloat("PosX", posicionLlegada.position.x);
            PlayerPrefs.SetFloat("PosY", posicionLlegada.position.y);
            PlayerPrefs.SetFloat("PosZ", posicionLlegada.position.z);
        }

        SceneManager.LoadScene(nombreEscenaDestino);
    }
}






