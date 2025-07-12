using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuCanvas;
    public GameObject volumeMenuUI;
    public GameObject pauseMenuUI;
    public GameObject keybindsMenuUI;
    private bool isPaused = false;
    public TMP_Text deathCounterText;


    private void Start()
    {
        deathCounterText.text = PlayerPrefs.GetInt("DeathCounter").ToString();
        pauseMenuCanvas.SetActive(false);
        volumeMenuUI.SetActive(false);  
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "StartMenu")
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuCanvas.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    void Pause()
    {
        pauseMenuCanvas.SetActive(true);
        volumeMenuUI.SetActive(false);
        keybindsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }
    public void LoadMainMenu()
    {
        Time.timeScale = 1f;
        PlayerPrefs.SetInt("LastSceneIndex", SceneManager.GetActiveScene().buildIndex);
        PlayerPrefs.SetInt("DeathCounter", int.Parse(deathCounterText.text));
        PlayerPrefs.Save();
        var inventory = GameObject.FindGameObjectWithTag("Inventory");
        if (inventory != null)
        {
            inventory.GetComponent<InventoryManager>().ResetInventory();
        }

        SceneManager.LoadScene("StartMenu");
    }

}
