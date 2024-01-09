using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
/* The base item class. All items should derive from this. */

public enum ItemCategory { helm, armor, leg, weapon, shield, consumable }
public enum ItemRarity { common, uncommon, rare, epic, legendary }

[CreateAssetMenu(fileName = "New Item", menuName = "Inventory/Item")]
public class Item : ScriptableObject {

	new public string name = "New Item";	// Name of the item
	public Sprite sprite = null;				// Item icon
   
    public ItemCategory currentItemCategory;
    public ItemRarity rarity;
    public Color[] rarityColor;
    public string itemID;
    public int usesLeft;
    public string actionName;
    public GameObject consumableEffect;

    public Dictionary<string, object> ItemAttributes = new Dictionary<string, object>();

    private string _instanceID = null;
    public string instanceID
    {
        get { return _instanceID; }
        set
        {
            if (_instanceID == null)
            {
                _instanceID = value;
            }
        }
    }


// public Color[] rarityColors;


// public string ItemID;


public Color getRarityColor() {

        switch (rarity)
        {
            case ItemRarity.common:
                return rarityColor[0];
                break;
            case ItemRarity.uncommon:
                return rarityColor[1];
                break;
            case ItemRarity.rare:
                return rarityColor[2];
                break;
            case ItemRarity.epic:
                return rarityColor[3];
                break;
            case ItemRarity.legendary:
                return rarityColor[4];
                break;

        }

        return Color.gray;
    }


    public Sprite getSlotImageByRarity()
    {

        switch (rarity)
        {
            case ItemRarity.common:
                return Inventory.instance.slotImages[1];
                break;
            case ItemRarity.uncommon:
                return Inventory.instance.slotImages[2];
                break;
            case ItemRarity.rare:
                return Inventory.instance.slotImages[3];
                break;
            case ItemRarity.epic:
                return Inventory.instance.slotImages[4];
                break;
            case ItemRarity.legendary:
                return Inventory.instance.slotImages[5];
                break;

        }

        return Inventory.instance.slotImages[0];
    }

    // Called when the item is pressed in the inventory
    public virtual void Use ()
	{
		// Use the item
		// Something may happen
	}

    public virtual void Consume()
    {
        // Use the item
        // Something may happen
    }

    public virtual void SelectItem()
    {
        

    }

    // Call this method to remove the item from inventory
    public void RemoveFromInventory ()
	{
		Inventory.instance.Remove(this);
	}

}
