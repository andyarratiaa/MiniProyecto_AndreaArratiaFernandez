using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Crosshair")]
    public GameObject crossHair;

    [Header("Ammo")]
    public TMP_Text currentAmmoCountText;
    public TMP_Text reservedAmmoCountText;

    public Image weaponIconImage; // Imagen del arma en la UI

    public void UpdateWeaponUI(Sprite newIcon, Vector2 newSize)
    {
        if (weaponIconImage != null && newIcon != null)
        {
            weaponIconImage.sprite = newIcon;
            RectTransform rectTransform = weaponIconImage.GetComponent<RectTransform>();
            if (rectTransform != null)
            {
                rectTransform.sizeDelta = newSize; // Ajusta el tamaño
            }
        }
    }
}

