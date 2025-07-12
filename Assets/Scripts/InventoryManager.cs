using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public ItemSlot[] itemSlots;
    [SerializeField] Sprite bootsImage;
    [SerializeField] Sprite dashImage;

    void Awake()
    {
        LoadPlayerItems();
    }
    public void AddItem(Sprite itemSprite)
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            if (itemSlots[i].isEmpty)
            {
                itemSlots[i].AddItem(itemSprite);
                break;
            }
        }
    }

    public void ResetInventory()
    {
        for (int i = 0; i < itemSlots.Length; i++)
        {
            itemSlots[i].ClearSlot();
        }
    }

    public void LoadPlayerItems()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) return; 
        else if (SceneManager.GetActiveScene().buildIndex == 1) return; 
        else if (SceneManager.GetActiveScene().buildIndex == 2) return; 
        else if (SceneManager.GetActiveScene().buildIndex == 3) itemSlots[0].AddItem(bootsImage);
        else if (SceneManager.GetActiveScene().buildIndex == 4){
            itemSlots[0].AddItem(bootsImage);
            itemSlots[1].AddItem(dashImage);
        } 
    }
}