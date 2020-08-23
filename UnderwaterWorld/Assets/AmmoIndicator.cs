using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AmmoIndicator : MonoBehaviour
{

    public Text ammoText;
    public Transform weaponHolder;
    private WeaponSwitching weaponSwitching;
    private Transform selectedWeapon;

    // Start is called before the first frame update
    void Start()
    {
        weaponSwitching = weaponHolder.GetComponent<WeaponSwitching>();
        selectedWeapon = weaponHolder.GetChild(weaponSwitching.selectedWeapon);
    }

    // Update is called once per frame
    void Update()
    {
        selectedWeapon = weaponHolder.GetChild(weaponSwitching.selectedWeapon);
        ammoText.text = string.Join("/", selectedWeapon.GetComponent<Gun>().GetCurrentAmmo(), selectedWeapon.GetComponent<Gun>().maxAmmo);
    }
}
