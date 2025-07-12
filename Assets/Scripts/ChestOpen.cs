using UnityEngine;
using TMPro;

public class ChestOpen : MonoBehaviour
{
    [SerializeField] private GameObject itemToDrop; 
    private Animator animator;
    private bool isOpened = false;
    public UIFade fade;
    AudioManager audioManager;
    public TMP_Text text;
    private bool playerClose = false;
    private void Awake()
    {
        animator = GetComponent<Animator>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }
    
    void Update()
    {
        if (!isOpened && playerClose && Keybinds.GetKeyDown("Interact"))
        {
            DropItem();
            fade.FadeOut();
            isOpened = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerClose = true;
            text.text = "Press " + Keybinds.GetKey("Interact") + " to open.";
            fade.FadeIn();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !isOpened)
        {
            playerClose = false;
            fade.FadeOut();
        }
    }

    private void DropItem()
    {
        if (itemToDrop != null)
        {
            audioManager.playSFX(audioManager.chestOpenSound);
            Instantiate(itemToDrop, transform.position + 5 * Vector3.right, Quaternion.identity);
            animator.SetTrigger("OpenChest");
        }
    }
}
