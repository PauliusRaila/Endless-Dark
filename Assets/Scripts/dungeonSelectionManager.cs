using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using PlayFab.CloudScriptModels;
using PlayFab;
using PlayFab.Json;

public class dungeonSelectionManager : MonoBehaviour
{
    public GameObject selectionLights;
    public GameObject lockedPrefab;

    public Button joinRoomBtn;
    private Dictionary<int, string> floorProgression = new Dictionary<int, string>();
    private List<GameObject> Floors = new List<GameObject>();
    public Camera tagCamera;


    //Selection manager UI;
    public Text floor;
    public Text progress;



    private int selectedFloor;
    public enum dungeonSize {
    
        Small,
        Medium,
        Large,
        
    }

    public dungeonSize selectedSize = dungeonSize.Medium;

    private void Start()
    {
        foreach (GameObject floor in GameObject.FindGameObjectsWithTag("Floor")) {
            Floors.Add(floor);
        }

        getFloorProgression();

        joinRoomBtn.onClick.AddListener(startFloor);
    }

    public void selectSize(int size) {
        switch (size) {
            case 0:
                selectedSize = dungeonSize.Small;
                break;
            case 1:
                selectedSize = dungeonSize.Medium;
                break;
            case 2:
                selectedSize = dungeonSize.Large;
                break;

        }

    }


    // Gets Unlocked, Locked and Completed floor progression of a player from PlayFab and applies it to dungeon floor selection.
    private void getFloorProgression()
    {
        PlayFabClientAPI.GetUserReadOnlyData(new PlayFab.ClientModels.GetUserDataRequest()
        {

        },
        result => {
        foreach (var item in result.Data)
            {
                if (item.Key.StartsWith("Floor")) {
                    floorProgression.Add(int.Parse(item.Key.Substring(6)), item.Value.Value);
           ///         Debug.Log(item.Key.Substring(6));               
           //         Debug.Log(floorProgression[int.Parse(item.Key.Substring(6))]);
                }
            }


            for (int i = 0; i < Floors.Count; i++) {
                switch (floorProgression[int.Parse(Floors[i].name)]) {
                    case "Unlocked":

                        break;
                    case "Completed":

                        break;
                    case "Locked":
                        Instantiate(lockedPrefab, Floors[i].transform);
                        break;
                
                }
            }
  
        
        },
        error => {
            Debug.Log("Got error getting read-only user data:");
            Debug.Log(error.GenerateErrorReport());
        });
    }



    private void startFloor()
    {
        PlayFabCloudScriptAPI.ExecuteEntityCloudScript(new ExecuteEntityCloudScriptRequest()
        {
            FunctionName = "startFloor",
            FunctionParameter = new
            {
                currentFloor = selectedFloor,
                currentSize = (int)selectedSize
            },
            GeneratePlayStreamEvent = true

        },
       result =>
       {
           gameManager.instance.sceneToLoad = "dungeon01_main";

           PUN_NetworkManager.nm.JoinRandomRoom();
        //   Debug.Log("Success, PhotonNetwork : Loading Level : " + PhotonNetwork.CurrentRoom.PlayerCount);
       }, LogFailure);
       
        //PhotonNetwork.LoadLevel(dungeon.dungeonID);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {

            Ray ray = tagCamera.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
               
               // if (hit.collider.tag == "Floor")
               // {

                  //  if (int.Parse(hit.collider.name) == selectedFloor || floorProgression[int.Parse(hit.collider.name)] == "Locked") return;

                 //   Destroy(GameObject.FindGameObjectWithTag("selectionLights"));
                 //   Instantiate(selectionLights, hit.collider.transform);
                //    selectedFloor = int.Parse(hit.collider.name);
                 //   floor.text = "Floor " + selectedFloor;
                //    progress.text = "[" + floorProgression[selectedFloor] + "]";

                    //hit.collider.transform.Find("Lights").gameObject.SetActive(true);
              //  }
                
            }
        }
    }


    void LogFailure(PlayFabError error)
    {
        Debug.LogError(error.GenerateErrorReport());


    }

}
