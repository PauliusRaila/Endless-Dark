using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class InventorySlot : MonoBehaviour {

	public Image icon;
	public Item item;   // Current item in the slot
	public GameObject stack;
	public GameObject stats;
	public Text mainAttribute;
	public Image indicator;



	// Add item to the slot
	public void AddItem (Item newItem)
	{
		item = newItem;
		icon.sprite = item.sprite;
		icon.enabled = true;
        GetComponentInChildren<Image>().color = Color.white;
        GetComponentInChildren<Image>().sprite = newItem.getSlotImageByRarity();

		if(stats != null)
		stats.SetActive(true);
		GetComponentInChildren<Button>().interactable = true;

		if (item.usesLeft > 0)
		{
			if (stack != null) {
				stack.SetActive(true);
				stack.GetComponentInChildren<Text>().text = item.usesLeft.ToString();
			}
			
		}
	}

	// Clear the slot
	public void ClearSlot ()
	{
		item = null;
		icon.sprite = null;
		icon.enabled = false;
        Color color = new Color(0.7f, 0.7f, 0.7f, 0.9f);
		if (stack.activeSelf)
			stack.SetActive(false);
        //0 = emptySlot
        GetComponentInChildren<Image>().sprite = Inventory.instance.slotImages[0];
        GetComponentInChildren<Image>().color = color;
		GetComponentInChildren<Button>().interactable = false;
		stats.gameObject.SetActive(false);
	}

	// If the remove button is pressed, this function will be called.
	public void RemoveItemFromInventory ()
	{
		Inventory.instance.Remove(item);
	}

    // Use the item
    public void selectItem()
    {
		foreach (Transform slot in menuManager.instance.itemsParent)
		{
			slot.GetComponentInChildren<Image>().sprite = menuManager.instance.commonSlot;
		}

		GetComponentInChildren<Image>().sprite = menuManager.instance.selectedSlot;

		menuManager.instance.selectedItemPanel.selectItem(item);
    }

    // Use the item
    public void UseItem ()
	{
		if (item != null)
		{
			item.Use();
		}
	}

}
