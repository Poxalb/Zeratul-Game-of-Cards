using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TextRotator : MonoBehaviour
{
    public TextMeshProUGUI textElement;
    public UIFade uiFade;
    public float interval = 2f;
    public int deaths;

    private string[] messages;

    void Start()
    {
        if (textElement == null || uiFade == null)
        {
            Debug.LogError("Text or UIFade not assigned!");
            return;
        }
        int deaths = PlayerPrefs.GetInt("DeathCounter");
        
        if (deaths < 10)
        {
            messages = new string[]
            {
                "The game has been succesfuly completed!",
                $"You only died {deaths} times, that's amazing! You are a mechanical prodigy",
                "You will now be redirected to the main menu"
            };
            
        }
        else if (deaths <30)
        {
            messages = new string[]
            {
                "The game has been succesfuly completed!",
                $"Wow! {deaths} deaths is quite a good score",
                "You will now be redirected to the main menu"
            };
            
        }
        else if (deaths < 60)
        {
            messages = new string[]
            {
                "The game has been succesfuly completed!",
                $"{deaths} deaths... You are a bit clumsy, but you made it!",
                "You will now be redirected to the main menu"
            }; 
        }
        else if (deaths < 100)
        {
            messages = new string[]
            {
                "The game has been succesfuly completed!",
                $"{deaths} deaths is actually quite a lot, you should try to turn on the screen next time! :)",
                "You will now be redirected to the main menu"
            };
            
        }
        else
        {
            messages = new string[]
            {
                "The game has been succesfuly completed!",
                $"Congratulations, you made it to the end with {deaths} deaths, for your efforts you have been awarded with...",
                "Absolutely nothing! \n\n\n You have entered the top 10 worst players leaderboard!",
                "You will now be redirected to the main menu" 
            };
        }

        uiFade.canvasGroup.alpha = 0f;
        textElement.gameObject.SetActive(false);

        StartCoroutine(ShowMessages());

        
    }

    IEnumerator ShowMessages()
    {
        foreach (string msg in messages)
        {
            textElement.text = msg;

            uiFade.FadeIn();
            yield return new WaitForSeconds(interval);

            uiFade.FadeOut();
            yield return new WaitForSeconds(uiFade.fadeDuration);
        }
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene("StartMenu");

    }
}
