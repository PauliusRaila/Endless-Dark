using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour {

    public delegate void  OnItemChanged();
	public OnItemChanged onItemChangedCallback;
    private InventoryUI ui;
    

    public static Inventory instance;

    public int space = 10;	// Amount of item spaces
    public int pages = 1;

    public List<Sprite> slotImages = new List<Sprite>();
    public static List<Item> items = new List<Item>();


    // Our current list of items in the inventory
    public List<Item> inventoryItems = new List<Item>();

    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else {

            Destroy(gameObject);

        }
       
        ui = GetComponent<InventoryUI>();
    }



    // Add a new item if enough room
    public void Add (Item item)
	{
            foreach (Item itm in inventoryItems)
            {
                if (itm == item)
                {
                    Debug.Log("found the same item");
                    item.usesLeft += 1;

                    if (onItemChangedCallback != null)
                        onItemChangedCallback.Invoke();
                    
                    return;
                }
            }

            inventoryItems.Add(item);

            if (onItemChangedCallback != null)
                onItemChangedCallback.Invoke();      
    }

	// Remove an item
	public void Remove (Item item)
	{
        if (item == null) return;

        if (item.usesLeft > 1)
        {
            item.usesLeft -= 1;
        }
        else {
            inventoryItems.Remove(item);


            if (menuManager.instance != null)
                if (menuManager.instance.selectedItemPanel.gameObject.activeSelf) {
                    menuManager.instance.selectedItemPanel.deselectItem(true);

                }
                           
        }

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();

    }

    public void clearInventory()
    {
        inventoryItems = new List<Item>();

        if (onItemChangedCallback != null)
            onItemChangedCallback.Invoke();
    }


}
