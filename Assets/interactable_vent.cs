using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorInteractable : MonoBehaviour, IInteractable
{
    public Transform door;
    public Vector3 openOffset = new Vector3(0f, 0f, 2f);
    public float moveSpeed = 2f;

    private Vector3 closedPosition;
    private Vector3 openPosition;
    private bool isOpen;

    public Animator animator;

    void Start()
    {
        closedPosition = door.position;
        openPosition = closedPosition + openOffset;
    }

    void Update()
    {
        Vector3 target = isOpen ? openPosition : closedPosition;

        door.position = Vector3.MoveTowards(
            door.position,
            target,
            moveSpeed * Time.deltaTime
        );
    }

    public void Interact()
    {
        isOpen = !isOpen;
        animator.SetTrigger("is interacting");
    }
}

