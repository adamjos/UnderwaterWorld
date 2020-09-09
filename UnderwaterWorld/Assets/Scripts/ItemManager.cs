using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{

    #region Singleton

    public static ItemManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } else
        {
            Destroy(gameObject);
            Debug.LogWarning("More than one instance of ItemManager found!");
            return;
        }
    }

    #endregion

    public bool[] pickedUpItems = new bool[5];

    public List<int> WeaponInventory = new List<int>();

    public event System.Action OnPickedUpItem;

    public int newItem = 0;

    private void Start()
    {
        // Always include 0, which corresponds to unarmed
        pickedUpItems[0] = true;
        WeaponInventory.Add(0);
    }


    public void AddItem (int slotID)
    {
        if (!pickedUpItems[slotID])
        {
            pickedUpItems[slotID] = true;
            WeaponInventory.Add(slotID);
            WeaponInventory.Sort();
            newItem = slotID;

            if (OnPickedUpItem != null)
            {
                OnPickedUpItem();
            }
        }
    }

}
