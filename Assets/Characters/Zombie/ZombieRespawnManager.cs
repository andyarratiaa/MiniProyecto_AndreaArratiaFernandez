//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;

//public class ZombieRespawnManager : MonoBehaviour
//{
//    [Header("Configuración")]
//    public GameObject zombieToActivate; // 🔹 Zombi que se activará cuando todos los demás mueran
//    private bool zombieActivado = false; // ✅ Para evitar activarlo varias veces

//    private List<ZombieManager> allZombies = new List<ZombieManager>();

//    private void Start()
//    {
//        // Buscar todos los zombis en la escena automáticamente
//        ZombieManager[] zombiesInScene = FindObjectsOfType<ZombieManager>();
//        allZombies.AddRange(zombiesInScene);

//        // Asegurarnos de que el zombi especial está desactivado al inicio
//        if (zombieToActivate != null)
//        {
//            zombieToActivate.SetActive(false); // 🔹 Lo dejamos inactivo al empezar
//        }
//    }

//    private void Update()
//    {
//        if (!zombieActivado) // ✅ Solo chequea si aún no se ha activado el nuevo zombi
//        {
//            CheckForAllZombiesDead();
//        }
//    }

//    private void CheckForAllZombiesDead()
//    {
//        // Filtrar zombis que aún están vivos
//        allZombies.RemoveAll(zombie => zombie == null || zombie.isDead);

//        // ✅ Si todos han muerto y el nuevo zombi aún no ha sido activado, inicia la espera
//        if (allZombies.Count == 0 && zombieToActivate != null)
//        {
//            Debug.Log("✅ Todos los zombis han sido eliminados. Esperando 15 segundos...");
//            StartCoroutine(ActivateZombieAfterDelay(15f)); // 🔹 Llamar a la corrutina
//        }
//    }

//    private IEnumerator ActivateZombieAfterDelay(float delay)
//    {
//        zombieActivado = true; // ✅ Evita que se active más de una vez
//        yield return new WaitForSeconds(delay); // ⏳ Espera 15 segundos
//        Debug.Log("🧟‍♂️ Activando el zombi...");
//        zombieToActivate.SetActive(true); // 🔹 Activa el zombi
//    }
//}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZombieRespawnManager : MonoBehaviour
{
    [Header("Configuración")]
    public GameObject zombieToActivate; // 🔹 Zombi que se activará cuando todos los demás mueran
    public GameObject endGameObject; // 🔹 Objeto que se activará cuando el último zombi muera
    public GameObject endGamePanel; // 🔹 Panel de fin de partida

    private bool zombieActivado = false;
    private bool endGameActivated = false; // ✅ Evita que el objeto final se active más de una vez

    private List<ZombieManager> allZombies = new List<ZombieManager>();

    private void Start()
    {
        // Buscar todos los zombis en la escena automáticamente
        ZombieManager[] zombiesInScene = FindObjectsOfType<ZombieManager>();
        allZombies.AddRange(zombiesInScene);

        // Asegurar que el zombi especial y el objeto de fin de partida están desactivados
        if (zombieToActivate != null)
        {
            zombieToActivate.SetActive(false);
        }

        if (endGameObject != null)
        {
            endGameObject.SetActive(false);
        }

        if (endGamePanel != null)
        {
            endGamePanel.SetActive(false); // El panel también comienza desactivado
        }
    }

    private void Update()
    {
        if (!zombieActivado)
        {
            CheckForAllZombiesDead();
        }
        else if (!endGameActivated)
        {
            CheckForFinalZombieDeath();
        }
    }

    private void CheckForAllZombiesDead()
    {
        // Filtrar zombis que aún están vivos
        allZombies.RemoveAll(zombie => zombie == null || zombie.isDead);

        // ✅ Si todos han muerto y el nuevo zombi aún no ha sido activado, inicia la espera
        if (allZombies.Count == 0 && zombieToActivate != null)
        {
            Debug.Log("✅ Todos los zombis han sido eliminados. Esperando 15 segundos...");
            StartCoroutine(ActivateZombieAfterDelay(15f));
        }
    }

    private IEnumerator ActivateZombieAfterDelay(float delay)
    {
        zombieActivado = true;
        yield return new WaitForSeconds(delay);
        Debug.Log("🧟‍♂️ Activando el último zombi...");
        zombieToActivate.SetActive(true);

        // Agregamos el último zombi a la lista para que podamos verificar su muerte
        ZombieManager finalZombie = zombieToActivate.GetComponent<ZombieManager>();
        if (finalZombie != null)
        {
            allZombies.Add(finalZombie);
        }
    }

    private void CheckForFinalZombieDeath()
    {
        // ✅ Si el último zombi está muerto, activar el objeto de fin de partida tras un tiempo
        if (zombieToActivate != null && zombieToActivate.GetComponent<ZombieManager>().isDead)
        {
            Debug.Log("✅ El último zombi ha muerto. Activando objeto de fin de partida...");
            StartCoroutine(ActivateEndGameObjectAfterDelay(10f));
            endGameActivated = true; // ✅ Evita que se active más de una vez
        }
    }

    private IEnumerator ActivateEndGameObjectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (endGameObject != null)
        {
            endGameObject.SetActive(true);
            Debug.Log("🎉 Objeto de fin de partida activado. ¡Ve a tocarlo!");
        }
    }
}


