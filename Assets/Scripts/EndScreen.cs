using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    public float delayBeforeClickable = 2f;
    
    private bool canClick = false;
    
    void Start()
    {
        Invoke(nameof(EnableClick), delayBeforeClickable);
    }
    
    void Update()
    {
        if (canClick && Input.GetMouseButtonDown(0))
        {
            SceneManager.LoadScene("StartMenu");
        }
    }
    
    private void EnableClick()
    {
        canClick = true;
    }
}