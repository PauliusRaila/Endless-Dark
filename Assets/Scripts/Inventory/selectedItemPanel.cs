using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using System;

public class selectedItemPanel : MonoBehaviour
{  
    public static selectedItemPanel instance { get; protected set; }

    public Text selectedItemTitle;
    public Text selectedItemRarity;
    public Text price;
    public GameObject attributePrefab;

    public Transform weaponStats;
    public Transform consumableStats;
    public Transform equipmentStats;

    public Image itemBackground;
    private Item selectedItem;
    public Image consumableItemIcon;
    public Button useButton;
    public Image background;
    public Image itemIcon;
    public equipedItemPanel equipedItemPanel;
    public Transform stack;

    public Transform helmPosition;
    public Transform armorPosition;
    public Transform legsPosition;
    public Transform weaponPosition;
    public Transform offHandPosition;

    public GameObject currentItem;
    public Item nextItem;

    public Material dissolveMaterial;
    public float dissolveValue = 1;
    public bool fadeOutRunning = false;
    public bool fadeInRunning = false;


    //TO-DO 

    private void OnLevelWasLoaded(int level)
    {
        if(level == 1)
            instance = this;
    }

        //Selected item fades out.
       
        

        //itemToFadeIn , itemToFadeOut



    private IEnumerator FadeOutObject() {

        fadeOutRunning = true;
                  
        Material mat = currentItem.GetComponent<MeshRenderer>().material;

           while (dissolveValue < 1)
           {
              dissolveValue += Time.deltaTime * 2f;
              mat.SetFloat("Vector1_FEFF47F1", dissolveValue);
              yield return null;
           }
       

        fadeOutRunning = false;
        initializeItem(nextItem);
      

        yield break;

    }

    private IEnumerator FadeInObject()
    {
        fadeInRunning = true;
        Material mat = currentItem.GetComponent<MeshRenderer>().material;

        while (dissolveValue > 0)
        {
            dissolveValue -= Time.deltaTime * 2f;
            mat.SetFloat("Vector1_FEFF47F1", dissolveValue);
            yield return null;
        }

        fadeInRunning = false;

        yield break;

        
    }



    public void selectItem(Item item)
    {
        helmPosition.parent.gameObject.SetActive(true);
        menuManager.instance.StartCoroutine(menuManager.instance.FadeImage(true, GetComponent<CanvasGroup>(), 3f));
       

        gameObject.SetActive(true);
        nextItem = item;

        if (currentItem == null)
        {
            initializeItem(nextItem);
            menuManager.instance.selectedItemPanel.GetComponent<CanvasGroup>().alpha = 0;
        }
        // If we select item while fading in, we want instantly fade out this item and fade in a new one.
        else if (fadeInRunning && !fadeOutRunning)
        {
            StopCoroutine(FadeInObject());
            fadeInRunning = false;
            StartCoroutine(FadeOutObject());
            StartCoroutine(FadeOutObject());
       //     Debug.Log("aaaaaaa");
        }

        // If while fading out we pick another item, we want to fade in that item instead of the old one.
        else if (fadeOutRunning && !fadeInRunning)
        {
            nextItem = item;
        //    Debug.Log("bbbbbbbb");
        }
        else if (!fadeInRunning && !fadeOutRunning) {
            StartCoroutine(FadeOutObject());
         //   Debug.Log("ccccccc");
        }
    }


    private void initializeItem(Item item) {
        if (item != null)
        {
           // Debug.Log("initializing itemmmmm");

        if(currentItem != null)
            currentItem.SetActive(false);


            equipmentStats.gameObject.SetActive(false);
            weaponStats.gameObject.SetActive(false);
            consumableStats.gameObject.SetActive(false);

            switch (item.currentItemCategory)
            {
                case ItemCategory.armor:
                    currentItem = armorPosition.gameObject;               
                    equipmentStats.gameObject.SetActive(true);
                    break;
                case ItemCategory.helm:
              
                    currentItem = helmPosition.gameObject;                     
                    equipmentStats.gameObject.SetActive(true);
                    break;
                case ItemCategory.leg:
                
                    currentItem = legsPosition.gameObject;
                    equipmentStats.gameObject.SetActive(true);
                    break;
                case ItemCategory.shield:
                  
                    currentItem = offHandPosition.gameObject;
                    equipmentStats.gameObject.SetActive(true);
                    break;
                case ItemCategory.weapon:
                    currentItem = weaponPosition.gameObject;
                    weaponStats.gameObject.SetActive(true);
                    break;
                case ItemCategory.consumable:
                    Inventory.instance.GetComponent<InventoryUI>().selectItem();
                    consumableStats.gameObject.SetActive(true);
                    consumableItemIcon.sprite = item.sprite;
                    break;
            }

            Inventory.instance.GetComponent<InventoryUI>().selectItem();


            if (item.currentItemCategory != ItemCategory.consumable) {
                currentItem.GetComponent<MeshFilter>().mesh = Resources.Load<Equipment>("Items/" + item.itemID).mesh;
                dissolveMaterial.SetTexture("Texture2D_A8228D34", Resources.Load<Equipment>("Items/" + item.itemID).mat.GetTexture("_BaseMap"));
                currentItem.GetComponent<MeshRenderer>().material = dissolveMaterial;
                currentItem.SetActive(true);
                StartCoroutine(FadeInObject());
            }   
          


            selectedItem = item;



            selectedItemRarity.color = selectedItem.getRarityColor();

            selectedItemTitle.text = selectedItem.name;
            selectedItemRarity.text = selectedItem.rarity + " " + selectedItem.currentItemCategory;

            itemIcon.sprite = selectedItem.sprite;

            foreach (KeyValuePair<string, object> attribute in selectedItem.ItemAttributes)
            {

                if (selectedItem.currentItemCategory == ItemCategory.weapon)
                {
                    weaponStats.Find(attribute.Key).Find("value").GetComponent<Text>().text = attribute.Value.ToString();
                    if (attribute.Key == "pDMG")
                        weaponStats.Find(attribute.Key).Find("bonusValue").GetComponent<Text>().text = "+" + StatsManager.instance.bonusPhysicalDamage.ToString();
                  //  else if (attribute.Key == "aDMG")
                   //     weaponStats.Find(attribute.Key).Find("bonusValue").GetComponent<Text>().text = "+" + StatsManager.instance.bonusArcaneDamage.ToString();

                }
                else if (selectedItem.currentItemCategory != ItemCategory.consumable)
                {

                }
                else if (selectedItem.currentItemCategory == ItemCategory.consumable)

                {


                }

                itemBackground.sprite = selectedItem.getSlotImageByRarity();

                useButton.onClick.RemoveAllListeners();





                if (selectedItem.currentItemCategory == ItemCategory.consumable)
                {
                    useButton.onClick.AddListener(() => item.Use());
                    useButton.GetComponentInChildren<Text>().text = "ADD TO STASH";
                    price.gameObject.SetActive(false);
                }
                else if (EquipmentManager.instance.GetEquipment(selectedItem.currentItemCategory) == selectedItem && selectedItem.currentItemCategory != ItemCategory.consumable)
                {
                    useButton.onClick.AddListener(() => EquipmentManager.instance.Unequip(selectedItem.currentItemCategory, true));
                    useButton.GetComponentInChildren<Text>().text = "UNEQUIP";
                    price.gameObject.SetActive(false);
                }
                else
                {
                    useButton.onClick.AddListener(() => item.Use());
                    useButton.GetComponentInChildren<Text>().text = "EQUIP";
                    price.gameObject.SetActive(false);
                }
            }
        }



    }

    public void deselectItem(bool consumable) {
        if (currentItem != null) {
            Debug.Log("deselectItem");

            dissolveValue = 1;
            currentItem.GetComponent<MeshRenderer>().material.SetFloat("Vector1_FEFF47F1", 1);          
            currentItem = null;

        }

        helmPosition.parent.gameObject.SetActive(false);

        if (!consumable)
          Inventory.instance.GetComponent<InventoryUI>().back();
        else
          Inventory.instance.GetComponent<dungeonBagUI>().back();

    }


    private string getStringByAttribute() {
        return "a";
    }


}
