using UnityEngine;

public class WeaponSwitching : MonoBehaviour
{

    public int selectedWeapon = 0;

    // Start is called before the first frame update
    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        if (PauseMenu.GameIsPaused)
        {
            return;
        }

        int previousSelectedWeapon = selectedWeapon;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (selectedWeapon >= ItemManager.instance.WeaponInventory.Count - 1)
            {
                selectedWeapon = 0;
            } else
            {
                selectedWeapon++;
            }
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (selectedWeapon <= 0)
            {
                selectedWeapon = ItemManager.instance.WeaponInventory.Count - 1;
            } else
            {
                selectedWeapon--;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1) && ItemManager.instance.pickedUpItems[1])
        {
            selectedWeapon = 1; // Grappling gun
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && transform.childCount >= 3 && ItemManager.instance.pickedUpItems[2])
        {
            selectedWeapon = 2; // Pistol
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) && transform.childCount >= 4 && ItemManager.instance.pickedUpItems[3])
        {
            selectedWeapon = 3; // Rifle
        }

        if (Input.GetKeyDown(KeyCode.Alpha4) && transform.childCount >= 5 && ItemManager.instance.pickedUpItems[4])
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

}
