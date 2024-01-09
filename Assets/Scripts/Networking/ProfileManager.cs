using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine.UI;
using System;
using PlayFab.Json;
using SimpleJSON;

public class ProfileManager : MonoBehaviour
{
    [SerializeField]
    private Text health;

    [SerializeField]
    private Text stamina;

    [SerializeField]
    private Text displayName;
    [SerializeField]
    private Text levelText;
    [SerializeField]
    private Text experienceText;
    [SerializeField]
    private Text skillpointsText;
    [SerializeField]
    private Text vitalityText;
    [SerializeField]
    private Text enduranceTect;
    [SerializeField]
    private Text strenghtText;
    [SerializeField]
    private Text intelligenceText;
   // [SerializeField]
    //private Text arcaneText;
    [SerializeField]
    private Text physicalDamageText;

    [SerializeField]
    private Text physicalDefenseText;
    // [SerializeField]
    //private Text arcaneDamageText;

    [SerializeField]
    private GameObject weaponSideStats;
    [SerializeField]
    private GameObject armorSideStats;
    [SerializeField]
    private GameObject overallSideStats;

    public List<GameObject> addSkillpointButtons = new List<GameObject>();

    private List<CatalogItem> catalogItems = new List<CatalogItem>();

    [SerializeField]
    private Text soulsBalanceText;  

    private int soulsBalance;

    public static ProfileManager instance { get; protected set; }

    private void Start()
    {
        instance = this;
        GetUserData();
        //RunLogTest();
    }

   

    private void GetUserData()
    {

        PlayFabClientAPI.GetAccountInfo(new GetAccountInfoRequest(), result =>
        {
            GetPlayerProfile(result.AccountInfo.PlayFabId);
        }, error =>
        {
            Debug.Log("Failed to get account info");
            return;
        }, null);

    }

    private void GetPlayerProfile(string playFabId)
    {

        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest()
        {
            PlayFabId = playFabId,
            ProfileConstraints = new PlayerProfileViewConstraints()
            {
                ShowDisplayName = true
            }

        },
        result =>
        {         
            displayName.text = result.PlayerProfile.DisplayName;
            //level.text = statistics[0].Value.ToString();
            //experience.text = statistics[1].Value.ToString() + " / " + statistics[0].Value * 1.1f;

            PlayFabClientAPI.GetCatalogItems(new GetCatalogItemsRequest()
            {
                CatalogVersion = null
            },
            result1 =>
            {
                catalogItems = result1.Catalog;
                updateSoulsBalance();
                LoadPlayerInventory();

            }, error1 => Debug.LogError(error1.GenerateErrorReport()));


         
            //StatsManager.instance.GetStatistics();

        },
        error => Debug.LogError(error.GenerateErrorReport()));

    }







    //delete later
    public void addExperience() {

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "addExperience",
            FunctionParameter = new
            {
                xpAmount = 500
            },

            // handy for logs because the response will be duplicated on PlayStream
            GeneratePlayStreamEvent = true
        }, result =>
        {
            if (result.Error != null)
            {

                Debug.Log(string.Format("There was error in the Cloud Script function {0}:\n Error Code: {1}\n Message: {2}"
                , result.FunctionName, result.Error.Error, result.Error.Message));
               
            }
            else
            {

                Debug.Log("Cloud Script successful! Experience added.");
                StatsManager.instance.GetStatistics();

            }
        }, LogFailure

        ); 
    }

    public void SetStatistics() {

        health.text = StatsManager.instance.health.ToString();
        stamina.text = StatsManager.instance.stamina.ToString();

        levelText.text = StatsManager.instance.level.ToString();
        experienceText.text = StatsManager.instance.experience + " / " + LevelToXP(StatsManager.instance.level + 1);
        skillpointsText.text = StatsManager.instance.skillpoints.ToString();
        vitalityText.text = StatsManager.instance.vitality.ToString();
        enduranceTect.text = StatsManager.instance.endurnace.ToString();
        strenghtText.text = StatsManager.instance.strenght.ToString();
        intelligenceText.text = StatsManager.instance.intelligence.ToString();
        //arcaneText.text = StatsManager.instance.arcane.ToString();

       // arcaneDamageText.text = StatsManager.instance.arcaneDamage.ToString();
        physicalDamageText.text = StatsManager.instance.physicalDamage.ToString();
        physicalDefenseText.text = StatsManager.instance.physicalDefense.ToString();


        if (StatsManager.instance.skillpoints <= 0)
        {
            foreach (GameObject skillPointButton in addSkillpointButtons)
            {
                if(skillPointButton != null)
                skillPointButton.SetActive(false);
            }
        }
        else
        {
            foreach (GameObject skillPointButton in addSkillpointButtons)
            {
                if (skillPointButton != null)
                    skillPointButton.SetActive(true);
            }
        }




        //level.text = levelValue.ToString();
        


    }

    public void useSkillpoint(int skillIndex) {
    

        PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
        {
            FunctionName = "useSkillpoint",
            FunctionParameter = new
            {
                index = skillIndex
            },

            // handy for logs because the response will be duplicated on PlayStream
            GeneratePlayStreamEvent = true
        }, result =>
        {
            if (result.Error != null)
            {

                Debug.Log(string.Format("There was error in the Cloud Script function {0}:\n Error Code: {1}\n Message: {2}"
                , result.FunctionName, result.Error.Error, result.Error.Message));

            }
            else
            {

                Debug.Log("Cloud Script successful! Skillpoint Added.");
                StatsManager.instance.GetStatistics();

            }
        }, LogFailure

       );
    }




    public int Equate(double xp)
    {
        return (int)Math.Floor(
            xp + 300 * Math.Pow(2, xp / 7));
    }

    public int LevelToXP(int level)
    {
        double xp = 0;

        for (int i = 1; i < level; i++)
            xp += this.Equate(i);

        return (int)Math.Floor(xp / 4);
    }

    public void LoadPlayerInventory()
    {     
            PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
            {
                Inventory.instance.clearInventory();
                List<ItemInstance> ii = result.Inventory;
                foreach (ItemInstance invItem in ii) {
                    Item item = Instantiate(Resources.Load<Item>("Items/" + invItem.ItemId));
                    item.itemID = invItem.ItemId;
                    
                    foreach(CatalogItem a in catalogItems)
                    {
                        if (a.ItemId == item.itemID)
                        {                       
                            item.ItemAttributes = JSON.Parse(a.CustomData).ToString().dictionaryFromJson();
                        } 
                    }

                    item.usesLeft = invItem.RemainingUses.GetValueOrDefault();
                    item.instanceID = invItem.ItemInstanceId;

                                   
                    Inventory.instance.Add(item);

                }

                if (gameManager.instance.localPlayer == null) {
                    gameManager.instance.InitializeLocalPlayer();
                    EquipmentManager.instance.helmMeshSlot = GameObject.FindGameObjectWithTag("HelmSlot").GetComponent<SkinnedMeshRenderer>();
                    EquipmentManager.instance.armorMeshSlot = GameObject.FindGameObjectWithTag("ArmorSlot").GetComponent<SkinnedMeshRenderer>();
                    EquipmentManager.instance.legsMeshSlot = GameObject.FindGameObjectWithTag("LegsSlot").GetComponent<SkinnedMeshRenderer>();
                }



                gameManager.instance.InitializeEquipedItems();


            }, LogFailure);     
    }

 


    public void getSideStats(ItemCategory category) {
        if (category == ItemCategory.weapon)
        {
            
        }
        else if (category != ItemCategory.weapon && category != ItemCategory.consumable) { 
                
        }
    }

    public void getSideStats() //get overall stats
    {
        weaponSideStats.SetActive(false);
        armorSideStats.SetActive(false);
        overallSideStats.SetActive(true);

        overallSideStats.transform.Find("health").Find("value").GetComponent<Text>().text = StatsManager.instance.health.ToString();
        overallSideStats.transform.Find("stamina").Find("value").GetComponent<Text>().text = StatsManager.instance.stamina.ToString();
        overallSideStats.transform.Find("physDEF").Find("value").GetComponent<Text>().text = StatsManager.instance.physicalDefense.ToString();
       // overallSideStats.transform.Find("arcaneDMG").Find("value").GetComponent<Text>().text = StatsManager.instance.arcaneDamage.ToString();
        overallSideStats.transform.Find("physDMG").Find("value").GetComponent<Text>().text = StatsManager.instance.physicalDamage.ToString();


    }

    public void getFloorProgression() { 
        


    }


    public void updateSoulsBalance() {
        PlayFabClientAPI.GetUserInventory(new GetUserInventoryRequest(), result =>
        {
            soulsBalance = result.VirtualCurrency["SL"];
            soulsBalanceText.text = soulsBalance.ToString();

        }, LogFailure);
    }









    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
    }
}