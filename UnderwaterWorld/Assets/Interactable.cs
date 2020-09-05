using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    //public bool isCurrentlyInteractable = true;

    public float interactRadius = 3f;

    //private bool hasInteracted = false;

    public string lookAtText;

    public virtual void Interact ()
    {
        // This is meant to be overritten
        Debug.Log("Interacting with " + transform.name);
    }

}
