using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLoaderSlot : MonoBehaviour
{
    public GameObject currentWeaponModel;

    private void UnloadAndDestroyWeapon()
    {
        if (currentWeaponModel != null)
        {
            if (currentWeaponModel.scene.IsValid()) // Asegurar que está en la escena
            {
                Destroy(currentWeaponModel);
            }
        }
    }

    public void LoadWeaponModel(WeaponItem weapon)
    {
        UnloadAndDestroyWeapon();

        if (weapon == null || weapon.itemModel == null)
        {
            return;
        }

        // Instanciar solo si no está en la escena (evita duplicados)
        if (weapon.itemModel.scene.rootCount == 0)
        {
            GameObject weaponModel = Instantiate(weapon.itemModel, transform);
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
            currentWeaponModel = weaponModel;
        }
    }
}



