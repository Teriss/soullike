using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public WeaponManager wm;
    public WeaponData weaponDate;

    private void Awake() {
        weaponDate = GetComponent<WeaponData>();
    }
}
