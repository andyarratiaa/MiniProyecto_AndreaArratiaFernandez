using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlPuerta : MonoBehaviour
{
    Animator anim;

    public bool Dentro = false;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Dentro = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.tag == "Player")
        {
            Dentro = false;
        }
    }

    // Update is called once per frame
    void Update() 
    {
        if(Dentro && Input.GetKeyDown(KeyCode.E))
        {
            anim.SetBool("Abierto", true);
        }
    }

}
