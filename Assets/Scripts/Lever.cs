using UnityEngine;
public class Lever : MonoBehaviour
{
    public bool isOn = false;
    public Sprite spriteOn;
    public Sprite spriteOff;
    private SpriteRenderer spriteRenderer;
    AudioManager audioManager;
    private bool playerClose = false;
    
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        updateSprite();
    }
    private void Awake()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    void Update()
    {
        if (playerClose && Input.GetKeyDown(Keybinds.GetKey("Interact")))
        {
            Debug.Log("Lever pressed");
            switchLever();
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerClose = false;
        }
    }
    void updateSprite()
    {
        if (isOn)
            spriteRenderer.sprite = spriteOn;
        else
            spriteRenderer.sprite = spriteOff;
    }
    public void switchLever()
    {
        isOn = !isOn;
        updateSprite();
        audioManager.playSFX(audioManager.leverSound);
        Debug.Log("Lever switched");
    }
}
