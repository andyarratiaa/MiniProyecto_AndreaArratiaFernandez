using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerUIManager : MonoBehaviour
{
    [Header("Crosshair")]
    public GameObject crossHair;

    [Header("Ammo")]
    public TMP_Text currentAmmoCountText;
    public TMP_Text reservedAmmoCountText;
}

