using UnityEngine;

public class PauseMenu : MonoBehaviour
{
    public GameObject PausePanel;
    private bool isPaused = false;

    void Start()
    {
        PausePanel.SetActive(false); // Asegura que el menú está oculto al iniciar
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                Continue();
            else
                Pause();
        }
    }

    public void Pause()
    {
        PausePanel.SetActive(true);
        Time.timeScale = 0;
        isPaused = true;

        // Desactiva el control del personaje y la cámara
        FindObjectOfType<InputManager>().enabled = false;
        FindObjectOfType<PlayerCamera>().enabled = false;
    }

    public void Continue()
    {
        PausePanel.SetActive(false);
        Time.timeScale = 1;
        isPaused = false;

        // Reactiva los controles del personaje y la cámara
        FindObjectOfType<InputManager>().enabled = true;
        FindObjectOfType<PlayerCamera>().enabled = true;
    }
}


