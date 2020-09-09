using System.Collections.Generic;
using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon = 0;
    private List<int> cachedWeaponInventory = new List<int>();
    private bool[] cachedPickedUpItems;
    private int scrollIndex = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
        UpdateWeaponInventory();
        ItemManager.instance.OnPickedUpItem += UpdateWeaponInventory;
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        int previousSelectedWeapon = selectedWeapon;

        scrollIndex = cachedWeaponInventory.IndexOf(previousSelectedWeapon);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (scrollIndex >= cachedWeaponInventory.Count - 1)
            {
                scrollIndex = 0;
            } else
            {
                scrollIndex++;
            }
            selectedWeapon = cachedWeaponInventory[scrollIndex];
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (scrollIndex <= 0)
            {
                scrollIndex = cachedWeaponInventory.Count - 1;
            } else
            {
                scrollIndex--;
            }

            selectedWeapon = cachedWeaponInventory[scrollIndex];
        }


        if (Input.GetKeyDown(KeyCode.Alpha1) && cachedPickedUpItems[1])
        {
            selectedWeapon = 1; // Grappling gun
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 3 && cachedPickedUpItems[2])
        {
            selectedWeapon = 2; // Pistol
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4 && cachedPickedUpItems[3])
        {
            selectedWeapon = 3; // Rifle
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 5 && cachedPickedUpItems[4])
        {
            selectedWeapon = 4; // Sniper rifle
        }


        if (previousSelectedWeapon != selectedWeapon)
        {
            SelectWeapon();
        } 

    }

    void SelectWeapon ()
    {
        int i = 0;
        foreach (Transform weapon in transform)
        {
            if (i == selectedWeapon)
            {
                weapon.gameObject.SetActive(true);
            } else
            {
                weapon.gameObject.SetActive(false);
            }
            i++;
        }

    }

    void UpdateWeaponInventory ()
    {
        cachedWeaponInventory = ItemManager.instance.WeaponInventory;
        cachedPickedUpItems = ItemManager.instance.pickedUpItems;
        selectedWeapon = ItemManager.instance.newItem;
        scrollIndex = cachedWeaponInventory.IndexOf(selectedWeapon);
        SelectWeapon();
    }

}
