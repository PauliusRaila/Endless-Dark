using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class equipedItemPanel : MonoBehaviour
{ 
    public Text selectedItemTitle;
    public Text selectedItemRarity;
    public GameObject attributePrefab;
    public Transform selectedItemAttributes;
    public Image itemBackground;
    private Item selectedItem;
    public Image background;
    public Image itemIcon;
    
    public void selectItem(ItemCategory selectedCategory)
    {      
        Item item = null;        
        
        switch (selectedCategory) {
            case ItemCategory.helm:
                item = EquipmentManager.instance.GetEquipment(ItemCategory.helm);
                break;

            case ItemCategory.armor:
                item = EquipmentManager.instance.GetEquipment(ItemCategory.armor);
                break;

            case ItemCategory.leg:
                item = EquipmentManager.instance.GetEquipment(ItemCategory.leg);
                break;
            case ItemCategory.weapon:
                item = EquipmentManager.instance.GetEquipment(ItemCategory.weapon);
                break;
            case ItemCategory.shield:
                item = EquipmentManager.instance.GetEquipment(ItemCategory.shield);
                break;

        }

        if (item == null) {
            gameObject.SetActive(false);
            return;        
        }
       

            selectedItem = item;
            selectedItemRarity.color = item.getRarityColor();

            selectedItemTitle.text = selectedItem.name;
            selectedItemRarity.text = selectedItem.rarity + "   " + selectedItem.currentItemCategory;
            itemIcon.sprite = selectedItem.sprite;

            foreach (Transform child in selectedItemAttributes.transform)
            {
                GameObject.Destroy(child.gameObject);
            }

            foreach (KeyValuePair<string, object> attribute in item.ItemAttributes)
            {
                Text attributeText = Instantiate(attributePrefab).GetComponent<Text>();
                attributeText.transform.SetParent(selectedItemAttributes);
                itemAttribute atb;

                if (selectedItem.currentItemCategory == ItemCategory.weapon)
                {
                    atb = Resources.Load<itemAttribute>("WeaponAttributes/" + attribute.Key);
                    attributeText.text = atb.description;                   
                }
                else if (selectedItem.currentItemCategory != ItemCategory.consumable) {
                    atb = Resources.Load<itemAttribute>("ArmorAttributes/" + attribute.Key);
                    attributeText.text = atb.description;                   
                }

                attributeText.transform.localScale = Vector3.one;


                //CHANGE COLOR ONLY FOR LEGENDARY ATTRIBUTES
                //attributeText.color = selectedItem.getRarityColor();
            }

            switch (selectedItem.rarity) {
                case ItemRarity.common:
                    background.rectTransform.offsetMin = new Vector2(background.rectTransform.offsetMin.x, 150);                   
                    break;

                case ItemRarity.uncommon:
                    background.rectTransform.offsetMin = new Vector2(background.rectTransform.offsetMin.x, 120);
                    break;

                case ItemRarity.rare:
                    background.rectTransform.offsetMin = new Vector2(background.rectTransform.offsetMin.x, 103);
                    break;

                case ItemRarity.epic:
                    background.rectTransform.offsetMin = new Vector2(background.rectTransform.offsetMin.x, 80);
                    break;

                case ItemRarity.legendary:
                    background.rectTransform.offsetMin = new Vector2(background.rectTransform.offsetMin.x, 48);
                    break;

            }

            itemBackground.sprite = selectedItem.getSlotImageByRarity();
        
            gameObject.SetActive(true);
        
        

    }

    public void deselectItem() {
        Inventory.instance.GetComponent<InventoryUI>().back();
    }


    private string getStringByAttribute() {
        return "a";
    }

}
