using UnityEngine;

public class Item : MonoBehaviour
{
    [SerializeField] private Sprite sprite;

    private InventoryManager inventoryManager;
    private PlayerMovement playerMovement;
    AudioManager audioManager;

    private void Start()
    {
        inventoryManager = GameObject.FindGameObjectWithTag("Inventory").GetComponent<InventoryManager>();
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            inventoryManager.AddItem(sprite);
            applyItemEffect(this.gameObject);
            string itemName = this.gameObject.name.Replace("(Clone)", "");
            PopupNotification.Instance.ShowNotification("New item unlocked!: " + itemName);
            Destroy(gameObject);
            audioManager.playSFX(audioManager.itemPickupSound);
        }   
    }

    private void applyItemEffect(GameObject item)
    {
        if (item.tag == "DoubleJumpBoots"){
            playerMovement.overJumps = 1;
            PlayerPrefs.SetString("BootsItem", "true");
        }
        else if (item.tag == "DashItem"){
            playerMovement.canDash = true;
            PlayerPrefs.SetString("DashItem", "true");
        }
        else if (item.tag == "GatesGuide"){
            playerMovement.OpenGatesGuide = true;
            PlayerPrefs.SetString("GatesItem", "true");
        }
        
    }

    public void InventoryReset()
    {
        inventoryManager.ResetInventory();
    }

}
