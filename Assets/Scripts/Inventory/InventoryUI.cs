using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/* This object manages the inventory UI. */

public class InventoryUI : MonoBehaviour {

    public ItemCategory selectedCategory = ItemCategory.armor;
    public Transform itemsParent;	// The parent object of all the items.
    public Transform categoriesParent; // The parent object of categories.
    
    private int slotIndex = 0;
	Inventory inventory;    // Our current inventory
                            //public Text categoryTitle;


    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) {
            itemsParent = menuManager.instance.itemsParent;
            categoriesParent = menuManager.instance.categoriesParent;

            categoriesParent.Find("helm").GetComponentInChildren<Button>().onClick.AddListener(delegate { selectCategory("helm"); });
            categoriesParent.Find("armor").GetComponentInChildren<Button>().onClick.AddListener(delegate { selectCategory("armor"); });
            categoriesParent.Find("legs").GetComponentInChildren<Button>().onClick.AddListener(delegate { selectCategory("leg"); });
            categoriesParent.Find("shields").GetComponentInChildren<Button>().onClick.AddListener(delegate { selectCategory("shield"); });
            categoriesParent.Find("weapons").GetComponentInChildren<Button>().onClick.AddListener(delegate { selectCategory("weapon"); });
          //  btn.onClick.AddListener(() => { Function(param); OtherFunction(param); });
        }
    }


    void Start ()
	{
		inventory = Inventory.instance;
        


        inventory.onItemChangedCallback += UpdateUI;
        
        //UpdateUI();	
	}

	// Update the inventory UI by:
	//		- Adding items
	//		- Clearing empty slots
	// This is called using a delegate on the Inventory.
	public void UpdateUI ()
	{
        if (SceneManager.GetActiveScene().name != "main_menuv2")
            return;

		        InventorySlot[] slots = itemsParent.GetComponentsInChildren<InventorySlot>();

                for (int i = 0; i < slots.Length; i++)
                {
                    slots[i].ClearSlot();
                }

                slotIndex = 0;

                foreach (Item item in inventory.inventoryItems)
                {

                    if (item.currentItemCategory == selectedCategory)
                    {
                        slots[slotIndex].AddItem(item);                        
                        slotIndex += 1;
                    }

                }

        if (selectedCategory != ItemCategory.consumable)
            updateEquipedSlot();
        
        slotIndex = 0;
	}



    public void selectCategory(string category)
    {
        menuManager.instance.StartCoroutine(menuManager.instance.FadeImage(true, categoriesParent.GetComponent<CanvasGroup>(), 3));
        ItemCategory parsed_enum = (ItemCategory)System.Enum.Parse(typeof(ItemCategory), category);
        selectedCategory = parsed_enum;
        categoriesParent.gameObject.SetActive(false);
        menuManager.instance.selectedItemPanel.gameObject.SetActive(false);


        itemsParent.parent.GetComponent<CanvasGroup>().alpha = 0;
        itemsParent.transform.parent.gameObject.SetActive(true);

        menuManager.instance.StartCoroutine(menuManager.instance.FadeImage(false, itemsParent.parent.GetComponent<CanvasGroup>() , 3));

        menuManager.instance.backButton.gameObject.SetActive(true);
        menuManager.instance.backButton.onClick.RemoveAllListeners();
        menuManager.instance.backButton.onClick.AddListener(back);


      
        UpdateUI();

    }

    public void selectItem()
    {      
        categoriesParent.gameObject.SetActive(false);
        gameManager.instance.localPlayer.SetActive(false);
        menuManager.instance.backButton.gameObject.SetActive(true);
        menuManager.instance.selectedItemPanel.GetComponent<CanvasGroup>().alpha = 0;
        menuManager.instance.selectedItemPanel.gameObject.SetActive(true);
        menuManager.instance.StartCoroutine(menuManager.instance.FadeImage(false, menuManager.instance.selectedItemPanel.GetComponent<CanvasGroup>(), 3));



    }

    public void back() {
        if (SceneManager.GetActiveScene().name == "main_menuv2") {          

            if (menuManager.instance.selectedItemPanel.currentItem != null)
            {
                menuManager.instance.selectedItemPanel.currentItem.GetComponent<MeshRenderer>().material.SetFloat("Vector1_FEFF47F1", 1);
                menuManager.instance.selectedItemPanel.dissolveValue = 1;
                menuManager.instance.selectedItemPanel.currentItem = null;

            }

            menuManager.instance.selectedItemPanel.gameObject.SetActive(false);
            gameManager.instance.localPlayer.SetActive(true);

          
            itemsParent.transform.parent.gameObject.SetActive(false);

            categoriesParent.GetComponent<CanvasGroup>().alpha = 0;
            categoriesParent.gameObject.SetActive(true);
            menuManager.instance.StartCoroutine(menuManager.instance.FadeImage(false, categoriesParent.GetComponent<CanvasGroup>(), 3));


            menuManager.instance.backButton.gameObject.SetActive(false);
            
            
            //ProfileManager.instance.getSideStats();
            
        }


    }

    public void backToInventory() {
    
        if (SceneManager.GetActiveScene().name == "main_menuv2") {
            UpdateUI();
            if (menuManager.instance.selectedItemPanel.currentItem != null)
            {
                menuManager.instance.selectedItemPanel.currentItem.GetComponent<MeshRenderer>().material.SetFloat("Vector1_FEFF47F1", 1);
                menuManager.instance.selectedItemPanel.dissolveValue = 1;
                menuManager.instance.selectedItemPanel.currentItem = null;

            }
            menuManager.instance.backButton.gameObject.SetActive(true);
            menuManager.instance.selectedItemPanel.gameObject.SetActive(false);
            itemsParent.transform.parent.gameObject.SetActive(true);
            gameManager.instance.localPlayer.SetActive(true);

        }
   
    }

    private void updateEquipedSlot() {
        equipedSlot eSlot = itemsParent.parent.GetComponentInChildren<equipedSlot>();
        eSlot.itemSlot = selectedCategory;

        switch (selectedCategory)
        {
            case ItemCategory.armor:
                if (EquipmentManager.instance.GetEquipment(ItemCategory.armor) != null)
                {                  
                        eSlot.AddItem(EquipmentManager.instance.GetEquipment(ItemCategory.armor), selectedCategory);                                  
                }else
                    eSlot.ClearSlot();
                break;
            case ItemCategory.helm:
                if (EquipmentManager.instance.GetEquipment(ItemCategory.helm) != null)
                {
                    eSlot.AddItem(EquipmentManager.instance.GetEquipment(ItemCategory.helm), selectedCategory);
                }
                else
                    eSlot.ClearSlot();

                break;
            case ItemCategory.leg:
                if (EquipmentManager.instance.GetEquipment(ItemCategory.leg) != null)
                {
                    eSlot.AddItem(EquipmentManager.instance.GetEquipment(ItemCategory.leg), selectedCategory);
                }
                else
                    eSlot.ClearSlot();

                break;
            case ItemCategory.shield:
                if (EquipmentManager.instance.GetEquipment(ItemCategory.shield) != null)
                {
                    eSlot.AddItem(EquipmentManager.instance.GetEquipment(ItemCategory.shield), selectedCategory);
                }
                else
                    eSlot.ClearSlot();

                break;
            case ItemCategory.weapon:
                if (EquipmentManager.instance.GetEquipment(ItemCategory.weapon) != null)
                {
                    eSlot.AddItem(EquipmentManager.instance.GetEquipment(ItemCategory.weapon), selectedCategory);
                }
                else
                    eSlot.ClearSlot();

                break;
        }
    }
}
