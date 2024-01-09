using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Invector.vCamera;
using Invector.vCharacterController;


public class dungeonBagManager : MonoBehaviour
{
    #region Singleton
    public static dungeonBagManager instance;
    #endregion



    //public delegate void OnItemChanged();
    //public OnItemChanged onItemChangedCallback;
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallback;
    public bool isDungeonBagOpen = false;
    public GameObject igDungeonBag = null;
    private GameObject mobilecontrols;



    private int space = 4;	// Amount of item spaces

    public static List<Item> items = new List<Item>();

    // Our current list of items in the bag.
    public List<dungeonBagSlot> dungeonBagSlots = new List<dungeonBagSlot>();

    // Our current list of items in the dungeonBag.
    public List<Item> dungeonBagItems = new List<Item>();


    void Awake()
    {
        foreach (GameObject slot in GameObject.FindGameObjectsWithTag("dungeonBagSlot"))
        {
            dungeonBagSlots.Add(slot.GetComponent<dungeonBagSlot>());
        }

        //Ensure the script is not deleted while loading
        DontDestroyOnLoad(this);

        //Make sure there are copies are not made of the GameObject when it isn't destroyed
        if (FindObjectsOfType(GetType()).Length > 1)
            //Destroy any copies
            Destroy(gameObject);

        instance = this;


    }


    private void OnLevelWasLoaded(int level)
    {
        if (level == 2) {
            if (igDungeonBag == null)
            {

                mobilecontrols = GameObject.Find("MobileControls");
                igDungeonBag = GameObject.FindGameObjectWithTag("dungeonBag");
                igDungeonBag.SetActive(false);


                if(dungeonBagItems.Count > 0)
                for (int i = 0; i <= dungeonBagItems.Count; i++) {
                    vHUDController.instance.stashSlots[i].AddItem(dungeonBagItems[i]);
                                      
                }


                

            }
        }
    }

    // Add a new item if enough room
    public void Add(Item item)
    {      
            if (dungeonBagItems.Count >= space)
            {
                Debug.Log("Bag is full.");
                return;
            }         

            if(!dungeonBagItems.Contains(item))
                dungeonBagItems.Add(item);

        foreach (dungeonBagSlot slot in dungeonBagSlots)
        {

            if (slot.item == item)
            {
                if (slot.amount >= space) {
                    Debug.Log("slot is full");
                    return;
                }
                          
                Debug.Log("found same item in slot, adding to stack");
                slot.AddItem(item);
                item.RemoveFromInventory();

                break;
            }
            else if (slot.item == null)
            {
                slot.AddItem(item);
                Debug.Log("Item added to new dungeon bag slot.");
                item.RemoveFromInventory();

                break;
            }
        }
    }


    //In-Game 
    public void ToggleDungeonBag()
    {
       
      

        if (isDungeonBagOpen)
        {


            gameManager.instance.localPlayer.GetComponent<vThirdPersonInput>().ChangeCameraState("Default");
            igDungeonBag.SetActive(false);
          
            mobilecontrols.SetActive(true);

            
            isDungeonBagOpen = false;
        }
        else
        {
            gameManager.instance.localPlayer.GetComponent<vThirdPersonInput>().ChangeCameraState("Consumables");
            igDungeonBag.SetActive(true);
           
            mobilecontrols.SetActive(false);

            isDungeonBagOpen = true;


        }

    }

    // Remove an item
    public void Remove(Item item)
    {
        
        dungeonBagItems.Remove(item);       
        Inventory.instance.Add(item);
    }



}
