using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Invector.vCharacterController.AI;

using UnityEngine.Events;

public class EnemySpawner : MonoBehaviour
{
    public static EnemySpawner instance { get; protected set; }
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();//  Declare list

    public List<Transform> enemySpawnPositions = new List<Transform>();
    public List<Transform> openSpawnPoints = new List<Transform>();
    public List<Transform> upcomingSpawnPoints = new List<Transform>();
    // List<KeyValuePair<double>> enemyPropabilities = new List<KeyValuePair<double>>();

    private int typesToSpawn;
    public int totalEnemiesAlive;
    //public int[] enemyTypeAlive;

    public int enemiesToSpawn = 5;
    public float timeAfterWave = 60f;
    private float countdown;

    private void Awake()
    {
        if (instance = null) {
            instance = this;
        }

        GameObject screen = GameObject.Find("Screen");
        screen.transform.localScale = new Vector3(screen.transform.localScale.y * Screen.width / Screen.height, screen.transform.localScale.y, screen.transform.localScale.z);
    }

    public void StartWave(int checkRoom)
    {
        checkForSpawner(checkRoom);

        //   if (enemySpawnPositions.Count <= 0)
        //   {
        //       GameObject[] enemySpawnPoints = GameObject.FindGameObjectsWithTag("enemySpawnPoint");

        //       foreach (GameObject point in enemySpawnPoints)
        //       {
        //          enemySpawnPositions.Add(point.transform);
        //      }
        //   }



        if (openSpawnPoints.Count <= 0) {
            Debug.Log("No open spawn points found.");
            return;
        } 

        countdown = timeAfterWave;

       

        prepareSpawnPoints();
        StartCoroutine(startCountdownToWave());
    }

    public void checkForSpawner(int roomNumber) {
        GameObject[] rooms = GameObject.FindGameObjectsWithTag("Room");
        foreach (GameObject room in rooms) {
            if (int.Parse(room.name) == roomNumber) {
                if (room.transform.Find("EnemySpawner") != null)
                {
                    if (openSpawnPoints.Contains(room.transform.Find("EnemySpawner").GetChild(0))) return;
                        openSpawnPoints.Add(room.transform.Find("EnemySpawner").GetChild(0));
                }
                else {
                    Debug.Log("There is no spawner in room " + roomNumber);
                }
            }
        }
        

    }

   // public void setUpDoors() {
   //     GameObject[] doors = GameObject.FindGameObjectsWithTag("door");
   //
   //     foreach (GameObject door in doors)
   //     {
    //        GameObject[] actions = GameObject.FindGameObjectsWithTag("Action");
//
            //foreach (GameObject action in actions)
           // {
           //     GetComponent<vTriggerGenericAction>().OnDoAction.AddListener(StartWave);
           // }
  //      }
  //  }

    IEnumerator startCountdownToWave()
    {
       
        for (int i = (int)timeAfterWave; i > 0; i--)
        {
            Debug.Log("Countdown: " + countdown);
            countdown--;          
            yield return new WaitForSeconds(1f);
        }

        if (countdown == 0)
        {
            Debug.Log("Starting wave");
            spawnWave();
        }

        
    }

    private void prepareSpawnPoints()
    {
        //pick closest spawnpoints for player
        upcomingSpawnPoints = new List<Transform>();

        for (int i = 0; i < PhotonNetwork.CurrentRoom.PlayerCount; i++)
        {
          int index = Random.Range(0, openSpawnPoints.Count - 1);
          upcomingSpawnPoints.Add(openSpawnPoints[index]); 
        }

    }

    public void spawnWave()
    {  
      StartCoroutine(spawnEnemies());
    }


    IEnumerator spawnEnemies()
    {
            for (int a = 0; a < PhotonNetwork.CurrentRoom.PlayerCount; a++)
            {
                int index = Random.Range(0, openSpawnPoints.Count - 1);    //  Pick random element from the list
                Transform spawnPoint = openSpawnPoints[index]; 
                
                Debug.Log("spawnPoint: " + spawnPoint);
                //upcomingSpawnPoints.RemoveAt(a);   //  Remove chosen element             

                for (int b = 0; b < enemiesToSpawn; b++)
                {
                   
                    float R = Random.value * 100;
                    // Debug.Log(R);
                    float cumulative = 0.0f;

                
                    for (int i = 0; i < enemyPrefabs.Count; i++)
                    {

                        cumulative += enemyPrefabs[i].GetComponent<vControlAIMelee>().chanceToSpawn;
                       // Debug.Log(cumulative);
                        if (R < cumulative)
                        {
                        // Debug.Log("spawn");
                        if (totalEnemiesAlive >= enemiesToSpawn) yield break;

                        spawnEnemy(spawnPoint, enemyPrefabs[i].name);
                            yield return new WaitForSeconds(0.5f);

                        }
                    
                    }
                yield return new WaitForSeconds(0.5f);
            }
         }
    }
 

    private void spawnEnemy(Transform spawnPoint, string enemyType)
    {
        GameObject enemy = PhotonNetwork.Instantiate(enemyType, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<vControlAIMelee>().onDead.AddListener(remainingEnemies);
        totalEnemiesAlive++;
        Debug.Log("Enemy spawned");

    }

    public void remainingEnemies(GameObject target) {
        
        totalEnemiesAlive--;
        if (totalEnemiesAlive <= 0) {
            Debug.Log("No wave enemies left.");
        }
    }

}
