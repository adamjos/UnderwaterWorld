using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickUp : Interactable
{
    public string itemName;

    public int slotID;

    public override void Interact()
    {
        base.Interact();
        ItemManager.instance.AddItem(slotID);
        Destroy(gameObject);
    }

}
