using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : Interactable
{

    private DialogueTrigger dialogueTrigger;

    private void Start()
    {
        dialogueTrigger = GetComponent<DialogueTrigger>();
    }


    public override void Interact()
    {
        base.Interact();
        dialogueTrigger.TriggerDialogue();
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
