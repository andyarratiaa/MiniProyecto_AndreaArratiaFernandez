using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WeaponLoaderSlot : MonoBehaviour
{
    public GameObject currentWeaponModel;

    private void UnloadAndDestroyWeapon()
    {
        if (currentWeaponModel != null)
        {
            // Verifica que el objeto está instanciado en la escena y no es un asset
            if (currentWeaponModel.scene.IsValid())
            {
                Destroy(currentWeaponModel);
            }
        }
    }

    public void LoadWeaponModel(WeaponItem weapon)
    {
        UnloadAndDestroyWeapon();

        if (weapon == null)
        {
            return;
        }

        // Instanciar solo si es un prefab y no un asset en sí mismo
        if (weapon.itemModel != null && PrefabUtility.IsPartOfPrefabAsset(weapon.itemModel))
        {
            GameObject weaponModel = Instantiate(weapon.itemModel, transform);
            weaponModel.transform.localPosition = Vector3.zero;
            weaponModel.transform.localRotation = Quaternion.identity;
            weaponModel.transform.localScale = Vector3.one;
            currentWeaponModel = weaponModel;
        }
    }
}


