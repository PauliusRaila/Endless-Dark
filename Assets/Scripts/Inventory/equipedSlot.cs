using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class equipedSlot : MonoBehaviour {

	public Image icon;
    public Text itemTitle;
    public Text slotStatus;
    public Image attribute;
    public Image mainAttributeIcon;
    public Text mainAttributeValue;
    public Image itemBackground;
    public ItemCategory itemSlot;

	public Item item; // Current item in the slot

    // Add item to the slot
    public void AddItem (Item newItem, ItemCategory slot)
	{
        itemSlot = slot;
		item = newItem;
		icon.sprite = item.sprite;
		icon.enabled = true;
        itemBackground.sprite = newItem.getSlotImageByRarity();
        itemTitle.text = item.name;
        //slotStatus.text = "Equiped";

       

        if (gameObject.name == "equipedItemSlot") { }
          GetComponentInChildren<Button>().interactable = true;

    }

	// Clear the slot
	public void ClearSlot ()
	{
       
        if (item != null) {
            item = null;
            icon.sprite = null;
            icon.enabled = false;


            itemTitle.text = "";
           // slotStatus.text = "Empty";
          //  slotStatus.color = itemTitle.color;
        }
        
        if (gameObject.name == "equipedItemSlot")
         GetComponentInChildren<Button>().interactable = false;
    }

    // Use the item
    public void selectItem()
    {
        menuManager.instance.selectedItemPanel.selectItem(item);
    }

}
