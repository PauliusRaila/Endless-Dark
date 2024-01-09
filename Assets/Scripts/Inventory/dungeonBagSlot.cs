using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

/* Sits on all InventorySlots. */

public class dungeonBagSlot : MonoBehaviour {

	public Image icon;
	public Item item;   // Current item in the slot
	public int amount = 0;
	public GameObject stack;
	private Text amountText;
    

    private void Start()
    {
       
    }

    // Add item to the slot
    public void AddItem (Item newItem)
	{
		item = newItem;
		icon.sprite = item.sprite;
		icon.enabled = true;
		GetComponentInChildren<Image>().color = Color.white;
		amount += 1;
		if (item.usesLeft > 0)
		{
			stack.SetActive(true);
			if (amountText == null) {
				amountText = stack.GetComponentInChildren<Text>();
				amountText.text = amount.ToString();

			}
				
			else
				amountText.text = amount.ToString();
		}
	}

	// Clear the slot
	public void ClearSlot ()
	{
		item = null;
		icon.sprite = null;
		icon.enabled = false;
		//Color color = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		//0 = emptySlot
		//GetComponentInChildren<Image>().color = color;
		amount = 0;
		if (stack.activeSelf)
			stack.SetActive(false);


	}

	// If the remove button is pressed, this function will be called.
	public void RemoveItemFromBag ()
	{

        if (item != null) {

			if (amount > 1)
			{
				amount -= 1;
				amountText.text = amount.ToString();
				Inventory.instance.Add(item);
			}
			else if(amount == 1) {
				amount -= 1;
				amountText.text = amount.ToString();
				dungeonBagManager.instance.Remove(item);
				ClearSlot();
			}

			Inventory.instance.GetComponent<dungeonBagUI>().UpdateUI();
        }
        else
        {
            Debug.Log("This slot is empty!");
        }
		
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
