using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* This object manages the inventory UI. */

public class dungeonBagUI : MonoBehaviour {

    public ItemCategory selectedCategory = ItemCategory.consumable;
    public Transform itemsParent;	// The parent object of all the items.
   /// public Transform categoriesParent; // The parent object of categories.

    public Transform bagItems;
    public InventorySlot[] slots;

    private int slotIndex = 0;
	Inventory inventory;	// Our current inventory

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1)
        {
            itemsParent = menuManager.instance.utilitiesItemsParents;
            bagItems = menuManager.instance.bagItems;
        }
    }

    void Start ()
	{
        inventory = Inventory.instance;      
        
        UpdateUI();
        inventory.onItemChangedCallback += UpdateUI;
        selectCategory("consumable");
    }

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI ()
	{      
        if (SceneManager.GetActiveScene().name != "main_menuv2")
            return;
     
        slots = itemsParent.GetComponentsInChildren<InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i].ClearSlot();
        }

        slotIndex = 0;

        foreach (Item item in inventory.inventoryItems)
        {

            if (item.currentItemCategory == ItemCategory.consumable)
            {
                slots[slotIndex].AddItem(item);
                slotIndex += 1;
            }

        }

        slotIndex = 0;
	}

    public void selectCategory(string category)
    {
        ItemCategory parsed_enum = (ItemCategory)System.Enum.Parse(typeof(ItemCategory), category);
        selectedCategory = parsed_enum;
        menuManager.instance.selectedItemPanel.gameObject.SetActive(false);
        UpdateUI();
    }

    public void selectItem()
    {         
        itemsParent.gameObject.SetActive(false);
        menuManager.instance.backButton.gameObject.SetActive(true);
        menuManager.instance.backButton.onClick.RemoveAllListeners();
        menuManager.instance.backButton.onClick.AddListener(back);
        menuManager.instance.selectedItemPanel.gameObject.SetActive(true);
        menuManager.instance.selectedItemPanel.equipedItemPanel.gameObject.SetActive(false);
      
       
    }

    public void back() {
        menuManager.instance.selectedItemPanel.gameObject.SetActive(false);
        itemsParent.gameObject.SetActive(true);
        menuManager.instance.backButton.gameObject.SetActive(false);
        bagItems.gameObject.SetActive(true);
        gameManager.instance.localPlayer.SetActive(true);
    }

    

}
