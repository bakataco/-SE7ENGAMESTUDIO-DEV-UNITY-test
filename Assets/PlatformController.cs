using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour, IInteractable
{
    public PlatformHinge[] platforms;
    public Animator animator;

    public void Interact()
    {
        ToggleAllPlatforms();
        animator.SetTrigger("is interacting");
    }

    public void ToggleAllPlatforms()
    {
        foreach (PlatformHinge platform in platforms)
        {
            if (platform != null)
            {
                platform.Toggle();
            }
        }
    }
}
