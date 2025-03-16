using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieManagerGlobal : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject keyObject; // Asigna la llave en el Inspector

    private List<ZombieManager> allZombies = new List<ZombieManager>();

    private void Start()
    {
        // Buscar todos los zombis en la escena automáticamente
        ZombieManager[] zombiesInScene = FindObjectsOfType<ZombieManager>();

        // Guardarlos en la lista
        allZombies.AddRange(zombiesInScene);

        // Asegurarse de que la llave esté desactivada al inicio
        if (keyObject != null)
        {
            keyObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("⚠ No se ha asignado un objeto de llave en el inspector.");
        }
    }

    private void Update()
    {
        CheckForAllZombiesDead();
    }

    private void CheckForAllZombiesDead()
    {
        // Filtrar zombis que aún están vivos
        allZombies.RemoveAll(zombie => zombie == null || zombie.isDead);

        // Si todos los zombis han muerto, activar la llave
        if (allZombies.Count == 0 && keyObject != null && !keyObject.activeSelf)
        {
            Debug.Log("✅ Todos los zombis han sido eliminados. Activando la llave...");
            keyObject.SetActive(true);
        }
    }
}
