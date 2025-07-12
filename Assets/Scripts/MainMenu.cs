using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{

    public GameObject volumeMenuUI;
    
    private void Start()
    {
        volumeMenuUI.SetActive(false);
    }
    public void NewGame()
    {
        SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("DeathCounter", 0);
    }
    public void LoadLevel1()
    {
        SceneManager.LoadScene(1);
    }
    public void LoadLevel2()
    {
        if(PlayerPrefs.HasKey("LastSceneIndex"))
        {
            int sceneIndex = PlayerPrefs.GetInt("LastSceneIndex");
            if (sceneIndex >= 2) {
                SceneManager.LoadScene(sceneIndex);
            }
            else CantLoadLevelMessage(2);            
        }
        else
        {
            NoGameFoundMessage();
        }
    }
    public void LoadLevel3()
    {
        if(PlayerPrefs.HasKey("LastSceneIndex"))
        {
            int sceneIndex = PlayerPrefs.GetInt("LastSceneIndex");
            if(sceneIndex >= 3) SceneManager.LoadScene(sceneIndex); else CantLoadLevelMessage(3);            
        }
        else
        {
            NoGameFoundMessage();
        }
    }

    public void LoadLevel4()
    {
        if(PlayerPrefs.HasKey("LastSceneIndex"))
        {
            int sceneIndex = PlayerPrefs.GetInt("LastSceneIndex");
            if(sceneIndex >= 4) SceneManager.LoadScene(sceneIndex); else CantLoadLevelMessage(4);            
        }
        else
        {
            NoGameFoundMessage();
        }
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
    private void CantLoadLevelMessage(int level)
    {
        Debug.Log("Instance is null? " + (NotificationManager.Instance == null));
        NotificationManager.Instance.ShowNotification($"You need to complete level {level - 1} before you can access this level.");
    }
  
}
