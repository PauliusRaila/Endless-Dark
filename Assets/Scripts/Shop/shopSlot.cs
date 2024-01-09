using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;


/* Sits on all InventorySlots. */

public class shopSlot : MonoBehaviour {

    public Image icon;
    public Button selectItemButton;
    public string storeID;
    private Item item;  // Current item in the slot. Should automatically pick item type;
    private Equipment equipmentItem;
    public uint itemPrice;
    public Text itemPriceText;
    public string itemTitle;
    public string itemDescription;
    public Mesh itemMesh;
    public Material itemMat;

    public Color selectedColor;
  
    // Add item to the slot    
    public void AddItem(string itemID, string storeId)
    {
        item = Resources.Load<Item>("Items/" + itemID);
        //PLEASE REFACTOR
        if (item.currentItemCategory != ItemCategory.consumable) {
            equipmentItem = Resources.Load<Equipment>("Items/" + itemID);
            itemMesh = equipmentItem.mesh;
            itemMat = equipmentItem.mat;
        }
            


        storeID = storeId;
        icon.sprite = item.sprite;
        icon.enabled = true;
        selectItemButton.interactable = true;
        itemPriceText.text = itemPrice.ToString();
        itemTitle = item.name;
       
        
    }

    public void clearItem() {

        item = null;
        itemMesh = null;
        itemMat = null;
        storeID = null;
        icon.sprite = null;
        icon.enabled = false;
        selectItemButton.interactable = false;
        itemPriceText.text = null;
        itemTitle = null;

    }



    public void selectItem()
    {
        Shop.instance.selectedItemId = item.itemID;
        Shop.instance.selectedItemPrice = (int)itemPrice;
        menuManager.instance.selectedItemPanel.selectItem(item);
        menuManager.instance.selectedItemPanel.useButton.onClick.RemoveAllListeners();
        menuManager.instance.selectedItemPanel.useButton.onClick.AddListener(Shop.instance.DefinePurchase);
        menuManager.instance.selectedItemPanel.useButton.GetComponentInChildren<Text>().text = "BUY";
        menuManager.instance.selectedItemPanel.price.text = itemPrice.ToString();
        menuManager.instance.selectedItemPanel.price.gameObject.SetActive(true);




        for (int i = 0; i < Shop.instance.shopSlots.Count; i++)
        {
            Shop.instance.shopSlots[i].GetComponentInChildren<Image>().sprite = menuManager.instance.commonSlot;        
        }

        GetComponentInChildren<Image>().sprite = menuManager.instance.selectedSlot;

    }

    



    private void DefinePaymentCurrency(string orderId, string currencyKey)
    {
        var request = new PayForPurchaseRequest
        {
            OrderId = orderId, // orderId comes from StartPurchase above
            Currency = currencyKey, // User defines which currency they wish to use to pay for this purchase (all items must have a defined/non-zero cost in this currency)
            ProviderName = "Test"
        };
        PlayFabClientAPI.PayForPurchase(request, LogSuccess, LogFailure);
    }

    private void LogSuccess(PayForPurchaseResult result)
    {
        Debug.Log(result + " @@@ " + result.Status);
    }

    private void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());

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
