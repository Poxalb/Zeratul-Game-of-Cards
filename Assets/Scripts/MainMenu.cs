using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public GameObject volumeMenuUI;
    
    private void Start()
    {
    }
    public void NewGame()
    {
        SceneManager.LoadScene("CombatScene"); // Load combat scene directly
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene("Combat"); // Load combat scene directly
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit Game");
    }

    private void NoGameFoundMessage()
    {
        Debug.Log("No game found. Please start a new game.");
    }
  
}
