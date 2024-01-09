using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Invector.vCharacterController;
using Invector.vMelee;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Invector;

public class EquipmentManager : MonoBehaviour {

    #region Singleton


    public static EquipmentManager instance {
        get {
            if (_instance == null)
            {
                _instance = FindObjectOfType<EquipmentManager>();
            }


            return _instance;
        }
    }
    static EquipmentManager _instance;

    void Awake()
    {
        _instance = this;
    }

    #endregion

    public Equipment[] defaultWear;

    public Equipment[] currentEquipment;
    Mesh[] currentMeshes;


    public SkinnedMeshRenderer helmMeshSlot;
    public SkinnedMeshRenderer armorMeshSlot;
    public SkinnedMeshRenderer legsMeshSlot;

    //public vWeaponHolder weaponHolder;

    //Main Menu Inventory
    private equipedSlot helmSlotUI;
    private equipedSlot armorSlotUI;
    private equipedSlot legsSlotUI;
    private equipedSlot weaponSlotUI;
    private equipedSlot shieldSlotUI;
    public Transform weaponHandle = null;
    public Transform shieldHandle = null;
    public enum EquipSide { Left, Right }
    //public SkinnedMeshRenderer weaponSlot;
    //public SkinnedMeshRenderer shieldSlot;

    // Callback for when an item is equipped
    public delegate void OnEquipmentChanged(Equipment newItem, Equipment oldItem); 
    public event OnEquipmentChanged onEquipmentChangedCallback;
    Inventory inventory;

    private void OnLevelWasLoaded(int level)
    {
        if (level == 1) {

            helmSlotUI = menuManager.instance.categoriesParent.Find("helm").GetComponent<equipedSlot>();
            armorSlotUI = menuManager.instance.categoriesParent.Find("armor").GetComponent<equipedSlot>();
            legsSlotUI = menuManager.instance.categoriesParent.Find("legs").GetComponent<equipedSlot>();
            weaponSlotUI = menuManager.instance.categoriesParent.Find("weapons").GetComponent<equipedSlot>();
            shieldSlotUI = menuManager.instance.categoriesParent.Find("shields").GetComponent<equipedSlot>();

            onEquipmentChangedCallback = null;
        }
    }


    void Start()
    {
        inventory = Inventory.instance;





        // inventory.GetComponent<InventoryUI>().categoriesParent


        int numSlots = System.Enum.GetNames(typeof(ItemCategory)).Length;
        currentEquipment = new Equipment[numSlots - 1];
        currentMeshes = new Mesh[numSlots];

    



      //  EquipAllDefault();
    }

    public Equipment GetEquipment(ItemCategory slot) {
        return currentEquipment[(int)slot];
    }

    // Equip a new item
    public void Equip(Equipment newItem)
    {
        EquipItem(newItem);

        Equipment oldItem = null;

        // Find out what slot the item fits in
        // and put it there.
        int slotIndex = (int)newItem.currentItemCategory;

        // If there was already an item in the slot
        // make sure to put it back in the inventory
        if (currentEquipment[slotIndex] != null)
        {
            oldItem = currentEquipment[slotIndex];

            inventory.Add(oldItem);

        }

        currentEquipment[slotIndex] = newItem;


        if (newItem.mesh) {

            if (currentMeshes[slotIndex] != null)
            {
                //Destroy (currentMeshes [(int)slot]);
                currentMeshes[slotIndex] = null;
            }

            switch (newItem.currentItemCategory)
            {
                case ItemCategory.helm:
                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        helmSlotUI.AddItem(newItem, newItem.currentItemCategory);

                   
                    helmMeshSlot.sharedMesh = newItem.mesh;
                    helmMeshSlot.sharedMaterial = newItem.mat;

                    

                    PlayerPrefs.SetString("helmSlot", newItem.instanceID);
                    PlayerPrefs.Save();

                    Debug.Log(newItem.name + " equipped!");

                    //PhotonView photonView = PhotonView.Get(PUN_NetworkManager.nm.photonView);
                    //photonView.RPC("SpawnAllPlayers", RpcTarget.All);

                    break;
                case ItemCategory.armor:
                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        armorSlotUI.AddItem(newItem, newItem.currentItemCategory);

                    armorMeshSlot.sharedMesh = newItem.mesh;
                    armorMeshSlot.sharedMaterial = newItem.mat;

                    PlayerPrefs.SetString("armorSlot", newItem.instanceID);
                    PlayerPrefs.Save();
                    Debug.Log(newItem.name + " equipped!");
                    break;
                case ItemCategory.leg:
                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        legsSlotUI.AddItem(newItem, newItem.currentItemCategory);

                 
                    legsMeshSlot.sharedMesh = newItem.mesh;
                    legsMeshSlot.sharedMaterial = newItem.mat;

                    PlayerPrefs.SetString("legSlot", newItem.instanceID);
                    PlayerPrefs.Save();
                    Debug.Log(newItem.name + " equipped!");
                    break;
                case ItemCategory.weapon:
                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        weaponSlotUI.AddItem(newItem, newItem.currentItemCategory);

                    GameObject weapon = Instantiate(newItem.itemPrefab);

                    weaponHandle = GetHandler(gameManager.instance.localPlayer.transform, newItem.currentItemCategory);
                    
                    
                    //GameObject.FindGameObjectWithTag(weapon.GetComponent<vMeleeWeapon>().weaponHandle).transform;

                    weapon.transform.parent = weaponHandle;
                    weapon.transform.localPosition = Vector3.zero;
                    weapon.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    if(oldItem != null)
                        Destroy(gameManager.instance.localPlayer.GetComponent<vMeleeManager>().rightWeapon.gameObject);                   
                    gameManager.instance.localPlayer.GetComponent<vMeleeManager>().SetRightWeapon(weapon);

                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        weapon.transform.localScale = Vector3.one;

                    PlayerPrefs.SetString("weaponSlot", newItem.instanceID);
                    PlayerPrefs.Save();
                    Debug.Log(newItem.name + " equipped!");
                    

                   // weaponHolder.weaponObject = weapon;
                   // weaponHolder.holderObject = weapon.transform.Find("mesh").gameObject;


                    break;
                case ItemCategory.shield:
                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        shieldSlotUI.AddItem(newItem, newItem.currentItemCategory);

                    GameObject shield = Instantiate(newItem.itemPrefab);

                    shieldHandle = GetHandler(gameManager.instance.localPlayer.transform, newItem.currentItemCategory);
                    shield.transform.parent = shieldHandle;
                    shield.transform.localPosition = Vector3.zero;
                    shield.transform.localRotation = new Quaternion(0, 0, 0, 0);
                    gameManager.instance.localPlayer.GetComponent<vMeleeManager>().SetLeftWeapon(shield);

                    if (SceneManager.GetActiveScene().name == "main_menuv2")
                        shield.transform.localScale = Vector3.one;
                    //gameManager.instance.localPlayer.GetComponent<vMeleeManager>().SetLeftWeapon(shield);

                    PlayerPrefs.SetString("shieldSlot", newItem.instanceID);
                    PlayerPrefs.Save();
                    Debug.Log(newItem.name + " equipped!");
                    break;
            }

          

            // An item has been equipped so we trigger the callback
            if (onEquipmentChangedCallback != null)
                onEquipmentChangedCallback.Invoke( newItem,  oldItem);


            currentMeshes[slotIndex] = newItem.mesh;
        }
        //equippedItems [itemIndex] = newMesh.gameObject;
        // inventory.Remove(newItem);

    }

    public void EquipItem(Equipment item)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "equipItem",
            FunctionParameter = new
            {

                slot = item.currentItemCategory.ToString() + "Slot",
                instanceId = item.instanceID
                //instanceToUpdate = result.Items[0].ItemInstanceId
            }

            // handy for logs because the response will be duplicated on PlayStream
            //GeneratePlayStreamEvent = true
        }, result => {

            Debug.Log("Cloud Script successful!");


        }, LogFailure);

        //  result.Items[0].


    }

    public void UnequipItem(ItemCategory slot)
    {
        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "equipItem",
            FunctionParameter = new
            {

                slot = slot.ToString() + "Slot",
                instanceId = ""
                //instanceToUpdate = result.Items[0].ItemInstanceId
            }

            // handy for logs because the response will be duplicated on PlayStream
            //GeneratePlayStreamEvent = true
        }, result => {

            Debug.Log("Cloud Script successful!");


        }, LogFailure);

        //  result.Items[0].


    }

    public void Unequip(ItemCategory slot , bool playfab) {
        if (currentEquipment[(int)slot] != null)
        {

            if (playfab) {

                UnequipItem(slot);
                PlayerPrefs.DeleteKey(slot.ToString() + "Slot");
            }
          

            Equipment oldItem = currentEquipment[(int)slot];
            inventory.Add(oldItem);

           

            switch (slot)
            {
                case ItemCategory.helm:
                  //  helmMeshSlot = GameObject.FindGameObjectWithTag("HelmSlot").GetComponent<SkinnedMeshRenderer>();
                    helmMeshSlot.sharedMesh = defaultWear[0].mesh;
                    helmMeshSlot.sharedMaterial = defaultWear[0].mat;
                 
                 
                   if (gameManager.instance.sceneName == "main_menuv2")                   
                        helmSlotUI.ClearSlot();

                    break;
                case ItemCategory.armor:
                    //  armorMeshSlot = GameObject.FindGameObjectWithTag("ArmorSlot").GetComponent<SkinnedMeshRenderer>();
                    armorMeshSlot.sharedMesh = defaultWear[1].mesh;
                    armorMeshSlot.sharedMaterial = defaultWear[1].mat;
                    if (gameManager.instance.sceneName == "main_menuv2")
                        armorSlotUI.ClearSlot();
                    break;
                case ItemCategory.leg:
                 //   legsMeshSlot = GameObject.FindGameObjectWithTag("LegsSlot").GetComponent<SkinnedMeshRenderer>();
                    legsMeshSlot.sharedMesh = defaultWear[2].mesh;
                    legsMeshSlot.sharedMaterial = defaultWear[2].mat;
                    if (gameManager.instance.sceneName == "main_menuv2")
                        legsSlotUI.ClearSlot();
                    break;
                case ItemCategory.weapon:
                    if (gameManager.instance.sceneName == "main_menuv2") {
                        weaponSlotUI.ClearSlot();

                        if (gameManager.instance.localPlayer.GetComponent<vMeleeManager>().rightWeapon != null)
                            Destroy(gameManager.instance.localPlayer.GetComponent<vMeleeManager>().rightWeapon.gameObject);
                    }
                        
                    
                   
                    break;
                case ItemCategory.shield:
                    if (gameManager.instance.sceneName == "main_menuv2") {
                        shieldSlotUI.ClearSlot();

                    if(gameManager.instance.localPlayer.GetComponent<vMeleeManager>().leftWeapon != null)
                        Destroy(gameManager.instance.localPlayer.GetComponent<vMeleeManager>().leftWeapon.gameObject);
                    }
                     
                    break;

            }

            currentEquipment[(int)slot] = null;


          
            //
            // Equipment has been removed so we trigger the callback
            //   if (onEquipmentChangedCallback != null)
            //     onEquipmentChangedCallback.Invoke(null, oldItem);

           


            if (gameManager.instance.sceneName == "main_menuv2")
                Inventory.instance.GetComponent<InventoryUI>().backToInventory();
        }
    }

    public void UnequipAll(bool playfab)
    {
        int numSlots = System.Enum.GetNames(typeof(ItemCategory)).Length - 1;
        for (int i = 0; i < numSlots; i++) {
            ItemCategory slot = (ItemCategory)i;
            Unequip(slot, playfab);
        }


    }

    void EquipAllDefault() {
        foreach (Equipment e in defaultWear) {
            Equip(e);
        }
    }


    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());

    }

    public Transform GetHandler(Transform owner, ItemCategory category)
    {
        string foundHandler = "";
        string searchParent = "";
        switch (category)
        {
            case ItemCategory.shield:
                searchParent = "shieldHandler";
                break;
            case ItemCategory.weapon:
                searchParent = "1handHandler";
                break;
        }
     
        Transform handler = FindWithName(owner, searchParent);
       // Transform handler = FindWithName(rootHandler, foundHandler);

        Debug.Log(handler);
        return handler;
    }

    Transform FindWithName(Transform root, string Name)
    {
        Transform retVal = null;
        if (root.name == Name)
        {
            retVal = root;
        }
        foreach (Transform child in root)
        {
            if (child.gameObject.name == Name)
            {
                retVal = child;
            }
            if (retVal == null)
            {
                retVal = FindWithName(child, Name);
                if (retVal != null)
                {
                    break;
                }
            }
        }

        return retVal;
    }


}
