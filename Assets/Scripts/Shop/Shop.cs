using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;
using PlayFab;
using System;

public class Shop : MonoBehaviour {

	#region Singleton

	public static Shop instance;
    public GameObject dailyItems; 
    public List<shopSlot> shopSlots;
    public GameObject shopPanel;
    public GameObject shopVendors;
    string storeID;


    public int selectedItemPrice;
    public string selectedItemId;


	void Awake ()
	{
		instance = this;

    }

    #endregion

	public int space = 10;	// Amount of item spaces
    //public int pages = 1;

	// Our current list of items in the inventory
	public List<Item> items = new List<Item>();

    void GetStoreItems(string newStoreID)
    {
        var primaryCatalogName = "Equipment"; // In your game, this should just be a constant matching your primary catalog
        var storeId = newStoreID; // In your game, this should be a constant for a permanent store, or retrieved from titleData for a time-sensitive store
        var request = new GetStoreItemsRequest
        {
            CatalogVersion = primaryCatalogName,
            StoreId = storeId
        };

        PlayFabClientAPI.GetStoreItems(request, result => {
            List<StoreItem> storeItems = result.Store;
            int itemIndex = 0;
            foreach (StoreItem item in storeItems) {
                Debug.Log("itemID " + item.ItemId);
                shopSlots[itemIndex].itemPrice = item.VirtualCurrencyPrices["SL"];
                shopSlots[itemIndex].AddItem(item.ItemId, storeId);
                
                itemIndex++;
            }
          
        }, LogFailure);
    }

    // This is typically NOT how you handle success
    // You will want to receive a specific result-type for your API, and utilize the result parameters
    protected void LogSuccess(GetStoreItemsResult result)
    {
        var requestName = result.Request.GetType().Name;       
        Debug.Log(requestName + " successful");
    }




    public void selectVendor(int shopID) {

        foreach (shopSlot slot in shopSlots) {
            slot.clearItem();
        }

        switch (shopID) {
            case 0:
                break;
            case 1:
                
              //  Inventory.instance.selectedShopItemPanel.shopTitle.text = "Blacksmith Shop";
                break;
            case 2:
               // Inventory.instance.selectedShopItemPanel.shopTitle.text = "Alchemist Shop";
                break;
            case 3:
                break;



        }


        shopPanel.SetActive(true);
        GetStoreItems("00" + shopID.ToString());
        shopVendors.SetActive(false);
        storeID = "00" + shopID.ToString();


        menuManager.instance.backButton.onClick.RemoveAllListeners();
        menuManager.instance.backButton.onClick.AddListener(Inventory.instance.GetComponent<InventoryUI>().back);
        menuManager.instance.backButton.gameObject.SetActive(true);
        // Inventory.instance.selectedShopItemPanel.backButton.onClick.RemoveAllListeners();
        // Inventory.instance.selectedShopItemPanel.backButton.onClick.AddListener(closeSelectedItemPanel);
        // Inventory.instance.selectedShopItemPanel.backButton.gameObject.SetActive(true);


    }

    public void closeSelectedItemPanel()
    {
        
      //  Inventory.instance.selectedShopItemPanel.backButton.gameObject.SetActive(false);
       
    }


    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());

    }



  



    public void DefinePurchase()
    {
        var primaryCatalogName = "Equipment"; // In your game, this should just be a constant matching your primary catalog
        var storeId = storeID; // At this point in the process, it's just maintaining the same storeId used above
        var request = new StartPurchaseRequest
        {
            CatalogVersion = primaryCatalogName,
            StoreId = storeId,
            Items = new List<ItemPurchaseRequest> {
            // The presence of these lines are based on the results from GetStoreItems, and user selection - Yours will be more generic
            new ItemPurchaseRequest { ItemId = selectedItemId, Quantity = 1,},
        }
        };
        PlayFabClientAPI.StartPurchase(request, result => {

            Debug.Log("Purchase started: " + result.OrderId);
            MakePurchase(selectedItemId);
            
        }, LogFailure);

    }

    private void MakePurchase(string itemID)
    {

       // buy.SetActive(false);
      //  processing.SetActive(true);
      //  processing.transform.parent.GetComponent<Button>().interactable = false;
      //  TweenRotate();

        PlayFabClientAPI.PurchaseItem(new PurchaseItemRequest
        {
            // In your game, this should just be a constant matching your primary catalog

            CatalogVersion = "Equipment",
            StoreId = storeID,
            ItemId = itemID,
            Price = selectedItemPrice,
            VirtualCurrency = "SL"

        }, result => {
            Debug.Log("Item successfully bought.");
            ProfileManager.instance.updateSoulsBalance();


            //
            Item item = Resources.Load<Item>("Items/" + result.Items[0].ItemId);
            Inventory.instance.Add(item);
            //notification.instance.showNotification(item.sprite,item.name, 1);
            // ProfileManager.instance.LoadPlayerInventory();
        }, LogFailure);




    }

}
