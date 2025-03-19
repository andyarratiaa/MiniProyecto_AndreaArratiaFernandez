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

    [Header("Weapon UI")]
    public GameObject pistolIcon;  //Icono de la pistola
    public GameObject rifleIcon;   //Icono del rifle

    private void Start()
    {
        //Asegurar que al inicio la pistola esté activada
        UpdateWeaponUI("Pistol");
    }

    public void UpdateWeaponUI(string currentWeaponName)
    {
        // Desactiva todos los iconos
        pistolIcon.SetActive(false);
        rifleIcon.SetActive(false);

        //Activa el icono según el arma actual
        if (currentWeaponName == "Pistola")
        {
            pistolIcon.SetActive(true);
        }
        else if (currentWeaponName == "Rifle")
        {
            rifleIcon.SetActive(true);
        }
    }
}


