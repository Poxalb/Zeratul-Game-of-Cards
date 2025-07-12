using UnityEngine;
public class LevelDoor : MonoBehaviour
{
    public Lever[] leverOn;  
    public Lever[] leverOff;    
    AudioManager audioManager;

    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private bool open = false;

    void Update()
    {
        if (!open && CheckLevers())
        {
            openGate();
        }
    }

    bool CheckLevers()
    {
        foreach (var lever in leverOn)
        {
            if (!lever.isOn) return false;
        }

        foreach (var lever in leverOff)
        {
            if (lever.isOn) return false;
        }

        return true;
    }

    void openGate()
    {
        open = true;
        gameObject.SetActive(false);
        audioManager.playSFX(audioManager.openGateSound);
        PopupNotification.Instance.ShowNotification("Room completed! Gate unlocked.");
    }
}
