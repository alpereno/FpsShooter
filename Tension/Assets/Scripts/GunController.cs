using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour
{//manage things (equip gun etc.)
    [SerializeField] private Transform weaponHoldT;
    [SerializeField] private Gun startingGun;
    Gun equippedGun;

    private void Start()
    {
        if (startingGun != null)
        {
            equipGun(startingGun);
        }
    }

    public void equipGun(Gun gunToEquip) {
        if (equippedGun != null)
        {
            Destroy(equippedGun.gameObject);
        }
        equippedGun = Instantiate(gunToEquip, weaponHoldT.position, weaponHoldT.rotation) as Gun;
        equippedGun.transform.parent = weaponHoldT;
    }

    public void shoot() {
        if (equippedGun != null)
        {
            equippedGun.shoot();
        }
    }

    public void reload() {
        if (equippedGun != null)
        {
            equippedGun.reload();
        }
    }
}
