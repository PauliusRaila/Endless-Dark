using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Invector.vCharacterController;
using Photon;
using Photon.Realtime;
using Photon.Pun;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Invector.vCharacterController.vActions;
using PlayFab.CloudScriptModels;
using UnityEngine.Rendering;

public class gameManager : MonoBehaviourPunCallbacks
{

    [Tooltip("Distance")]
    public float distance = 20f;
    [Tooltip("Width")]
    public float frustumWidth = 40.3f;
    [Tooltip("Limit maximum FOV value")]
    public float maxFOV = 65;
    private Camera mainCamera;
    private float cameraAspect;


    public GameObject playerPrefab; // Load from resources later.
    public GameObject playerPrefabSP;
    public GameObject deathScreen;

    public GameObject localPlayer, otherPlayer;

    public string sceneToLoad;
  
    public string sceneName;

 

    public static gameManager instance { get; protected set; }

    public List<Transform> spawnPoints = new List<Transform>();

    public enum MatchType
    {
        Solo,
        Duo,
    }

    public MatchType matchType = MatchType.Solo;

  
    private void Awake()
    {

        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;
        SceneManager.sceneLoaded += OnSceneLoaded;
           
    }

    private void Start()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);

        }

        mainCamera = Camera.main;
        //CameraAdjust();
    }

    private void Update()
    {
    #if UNITY_EDITOR
       //CameraAdjust();
    #endif
    }

    /// <summary>
    /// Dynamically adjust the camera FOV so that the camera can see the same width field of view at different resolutions
    /// </summary>
    [ContextMenu("Execute")]
    private void CameraAdjust()
    {
        if (mainCamera == null) return;
        // Refresh only when the resolution changes
        if (cameraAspect.Equals(mainCamera.aspect)) return;

        cameraAspect = mainCamera.aspect;
        // Calculate Fov
        float _fov = 2 * Mathf.Atan(frustumWidth / cameraAspect * 0.5f / distance) * Mathf.Rad2Deg;
        // Prevent Fov from being too large, set the maximum value
        if (maxFOV > 0 && _fov > maxFOV)
        {
            _fov = maxFOV;
        }
        mainCamera.fieldOfView = _fov;
    }


    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        sceneName = scene.name;

        RenderTexture rt;
        GameObject screen = GameObject.Find("Screen");
        screen.transform.localScale = new Vector3(screen.transform.localScale.y * Screen.width / Screen.height, screen.transform.localScale.y, screen.transform.localScale.z);

        float aspectRatio = Screen.width / (float)Screen.height;

        if (aspectRatio >= 2)
        {
            Debug.Log("18:9");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else if (aspectRatio >= 1.7f)
        {
            Debug.Log("16:9");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else if (aspectRatio >= 1.5f)
        {
            Debug.Log("3:2 FIX THIS");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }
        else
        {
            Debug.Log("4:3 FIX THIS");
            int width = Mathf.RoundToInt(256);
            int height = Mathf.RoundToInt(width / aspectRatio);
            rt = new RenderTexture(width, height, 24);
        }

        rt.antiAliasing = 1;
        rt.dimension = TextureDimension.Tex2D;
        rt.format = RenderTextureFormat.ARGB64;
        rt.useDynamicScale = false;
        rt.filterMode = FilterMode.Point;
        rt.Create();

        Camera.main.targetTexture = rt;

        GameObject.Find("Screen").GetComponent<MeshRenderer>().material.SetTexture("_BaseMap", rt);


        if (scene.name == "main_menuv2")
        {        

            spawnPoints = new List<Transform>();
            localPlayer = null;
            Debug.Log(scene.name + " scene is loaded.");

        }
        else if (scene.name == "dungeon01_main") {
           
        }
      
    }

   


    public void InitializeEquipedItems() {
      
        Debug.Log("InitializeEquipedItems");
        Item helm = null;
        Item armor = null;
        Item leg = null;
        Item weapon = null;
        Item shield = null;

        EquipmentManager.instance.UnequipAll(false);

        foreach (Item invItem in Inventory.instance.inventoryItems)
        {
            if (invItem.instanceID == PlayerPrefs.GetString("helmSlot")) {
                helm = invItem;
            }
            else if (invItem.instanceID == PlayerPrefs.GetString("armorSlot")) {              
                armor = invItem;
            }           
            else if (invItem.instanceID == PlayerPrefs.GetString("legSlot"))               
            {                
                leg = invItem;
            }
            else if (invItem.instanceID == PlayerPrefs.GetString("weaponSlot"))
            {
                weapon = invItem;
            }
            else if (invItem.instanceID == PlayerPrefs.GetString("shieldSlot"))
            {
                shield = invItem;
            }
        }

        if (helm != null)
            helm.Use();
        if (armor != null)
            armor.Use();
        if (leg != null)
            leg.Use();
        if (weapon != null)
            weapon.Use();
        if (shield != null)
            shield.Use();

        StatsManager.instance.GetStatistics();
        Inventory.instance.GetComponent<InventoryUI>().back();

    }


    public void InitializeDeath(GameObject arg0)
    {
        Debug.Log("Player died!");
        UserDataRecord testUserDataRecord;
        List<string> equipedItems = new List<string> { "armorSlot", "helmSlot", "legSlot", "shieldSlot", "weaponSlot" };

        PlayFabClientAPI.GetUserReadOnlyData(new GetUserDataRequest
        {
            Keys = equipedItems

        }, result1 => {

            List<string> itemsToDestroy = new List<string>();

            foreach (string slotId in equipedItems) {
                if (result1.Data.TryGetValue(slotId, out testUserDataRecord))
                {                 

                    if (testUserDataRecord.Value != "" || testUserDataRecord.Value != null) {
                        Debug.Log(testUserDataRecord.Value);
                        itemsToDestroy.Add(testUserDataRecord.Value);
                    }
                    
                }

            }

            PlayFabClientAPI.ExecuteCloudScript(new ExecuteCloudScriptRequest
            {
                FunctionName = "initializeDeath",
                FunctionParameter = new
                {
                    instancesToDestroy = itemsToDestroy

                },
                GeneratePlayStreamEvent = true

                // handy for logs because the response will be duplicated on PlayStream

            }, result2 => {

                Debug.Log("Death successful!");


                EquipmentManager.instance.UnequipAll(true);





                GameObject ds;
                ds = Instantiate(deathScreen, GameObject.FindGameObjectWithTag("PlayerUI").transform);
                ds.GetComponent<Button>().onClick.AddListener(returnToMenu);
               
                
                


            }, LogFailure); 

        }, LogFailure);
         
    }

    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());
       

    }
    public Transform getSpawnPoint(bool isMine) {
        spawnPoints = new List<Transform>();

        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("spawnPoint"))
        {
            spawnPoints.Add(spawnPoint.transform);
        }

        if (PhotonNetwork.IsMasterClient)
            return spawnPoints[0];
        else
            return spawnPoints[1];

    }

    public void InitializeLocalPlayer() {

        spawnPoints = new List<Transform>();
        foreach (GameObject spawnPoint in GameObject.FindGameObjectsWithTag("spawnPoint"))
        {            
            spawnPoints.Add(spawnPoint.transform);
        }
      
        localPlayer = null;      
        localPlayer = PUN_NetworkManager.nm.SpawnPlayer();     

    }



    public void returnToMenu() {
        PhotonNetwork.LeaveLobby();      
        PhotonNetwork.Destroy(localPlayer);
        PhotonNetwork.LoadLevel(1);
        Debug.Log("Return to main menu.");
    }

    public void setDeathButton(GameObject button) {
        button.GetComponent<Button>().onClick.AddListener(returnToMenu);
    }



    //Always change sceneToLoad befor calling LoadLevel;
    public void LoadLevel() {
        StartCoroutine(menuManager.instance.FadeImage(true, menuManager.instance.mainCanvas, 4));
        SceneManager.LoadScene("LoadingScreen", LoadSceneMode.Additive);     
    }

}
