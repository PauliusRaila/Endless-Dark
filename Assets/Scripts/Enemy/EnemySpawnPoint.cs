using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Invector.vCharacterController.AI;

using UnityEngine.Events;

public class EnemySpawnPoint : MonoBehaviour
{
    public List<GameObject> enemyPrefabs = new List<GameObject>();
    private List<GameObject> spawnedEnemies = new List<GameObject>();//  Declare list

    private int typesToSpawn;
    public int totalEnemiesAlive;

    public int enemiesToSpawn = 1;


    public void Start()
    {
        StartCoroutine(spawnEnemies());
    }

    IEnumerator spawnEnemies()
    {
          
                //int index = Random.Range(0, upcomingSpawnPoints.Count - 1);    //  Pick random element from the list                          
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

                            spawnEnemy(this.transform, enemyPrefabs[i].name);
                            yield return new WaitForSeconds(1f);

                        }
                    
                    }
                yield return new WaitForSeconds(1f);
            }
         }
    
 

    private void spawnEnemy(Transform spawnPoint, string enemyType)
    {
        GameObject enemy = PhotonNetwork.Instantiate(enemyType, spawnPoint.position, spawnPoint.rotation);
        enemy.GetComponent<vControlAIMelee>().onDead.AddListener(remainingEnemies);
        totalEnemiesAlive++;
        Debug.Log("Room enemy spawned");

    }

    public void remainingEnemies(GameObject target) {
        
        totalEnemiesAlive--;
        if (totalEnemiesAlive <= 0) {
            Debug.Log("All room enemies died.");
        }
    }

}
