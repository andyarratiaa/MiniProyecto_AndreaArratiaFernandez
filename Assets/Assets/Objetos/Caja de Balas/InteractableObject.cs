using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    // The base class for interactable objects (Items, Doors, Levers, Etc)

    protected PlayerManager player; // The player interacting with the object
    [SerializeField] protected GameObject interactableImage; // The image indicating a player can interact with this object
    protected Collider interactableCollider; // The collider enabling interaction when the player is close enough for interaction

    // Unity Message
    private void OnTriggerEnter(Collider other)
    {
        // OPTIONAL: Check for specific layer of collider

        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableImage.SetActive(true);
        }
    }

    // Unity Message
    private void OnTriggerExit(Collider other)
    {
        // OPTIONAL: Check for specific layer of collider

        if (player == null)
        {
            player = other.GetComponent<PlayerManager>();
        }

        if (player != null)
        {
            interactableImage.SetActive(false);
        }
    }
}

