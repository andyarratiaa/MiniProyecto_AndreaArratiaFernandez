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

public class ControlPuerta : MonoBehaviour
{
    Animator anim;
    public bool Dentro = false;
    bool puerta = false;
    public bool tieneLlave = false; // Esta puerta solo se abre si tiene su llave correspondiente

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
            if (tieneLlave) // Verifica si la puerta tiene la llave correcta
            {
                puerta = !puerta;
                anim.SetBool("Abierto", puerta);
            }
            else
            {
                Debug.Log("Necesitas la llave correcta para abrir estas puertas.");
            }
        }
    }

    // Método para asignar la llave a esta puerta (lo llamará el script de la llave)
    public void ObtenerLlave()
    {
        tieneLlave = true;
        Debug.Log("Has obtenido la llave para estas puertas.");
    }
}





