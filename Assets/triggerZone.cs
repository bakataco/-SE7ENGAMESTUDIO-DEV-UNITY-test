using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionZone : MonoBehaviour
{
    public MonoBehaviour interactableObject;

    private IInteractable interactable;

    private void Awake()
    {
        interactable = interactableObject as IInteractable;
    }

    private void OnTriggerEnter(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null && interactable != null)
        {
            player.SetInteractable(interactable);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        Player player = other.GetComponentInParent<Player>();

        if (player != null && interactable != null)
        {
            player.ClearInteractable(interactable);
        }
    }
}
