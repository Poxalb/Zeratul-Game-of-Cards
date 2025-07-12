using UnityEngine.UI;
using UnityEngine;


public class ItemSlot : MonoBehaviour
{
    public Sprite itemSprite; 
    public bool isEmpty = true;
    [SerializeField] public Image itemImage;

    public void AddItem(Sprite itemSprite)
    {
        this.itemSprite = itemSprite;
        isEmpty = false;
        itemImage.color = Color.white;
        itemImage.sprite = itemSprite;
    }   

    public void ClearSlot()
    {
        itemSprite = null;
        isEmpty = true;
        itemImage.sprite = null;
        itemImage.color = new Color(1, 1, 1, 0);
    }

}
