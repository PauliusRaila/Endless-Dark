using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileManager : MonoBehaviour {

    public List<GameObject> tilePrefabs = new List<GameObject>();

    private Transform playerTransform;
    private float spawnZ , spawnX;
    public float tileLenght = 20f;

    private int amountOfTiles = 3;
    private int centerRowIndex, centerTileIndex;

    public Transform centerTile;

    [SerializeField]
    public Dictionary<int, List<GameObject>> activeTiles = new Dictionary<int, List<GameObject>>();



    private void Start () {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        spawnZ = tileLenght * -1;
        spawnX = tileLenght * -1;

        SpawnTiles();
   

    }

    private void FixedUpdate()
    {
        UpdateTiles();

    }

    private void SpawnTiles() {
        


        for (int tempRow = 0; tempRow < amountOfTiles; tempRow++) {
            activeTiles[tempRow] = new List<GameObject>();
            Debug.Log("Amount of Rows: " + activeTiles.Count);


            for (int tempTile = 0; tempTile < amountOfTiles; tempTile++) {
                GameObject tile;

                if (tempRow == 1 && tempTile == 1)
                    tile = Instantiate(tilePrefabs[2]) as GameObject;
                else
                    tile = Instantiate(pickRandomTile()) as GameObject;

                tile.transform.SetParent(transform);

                tile.transform.position = new Vector3(spawnX, 0, spawnZ);

                
                spawnX += tileLenght;
               
                activeTiles[tempRow].Add(tile);

                if (tempRow == 1 && tempTile == 1) {
                    centerTile = activeTiles[tempRow][tempTile].transform;
                    centerRowIndex = tempRow;
                    centerTileIndex = tempTile;
                }
                    
            }           
           
            spawnZ += tileLenght;
            spawnX = -tileLenght;
        }

        spawnZ = centerTile.position.z + tileLenght * 2;
    }

    private void UpdateTiles() {
        //Move UP
        if (playerTransform.position.z >= (centerTile.transform.position.z + tileLenght))
        {
            DeleteRow(0);

            for (int tile = 2; tile >= 0; tile--)
            {
                activeTiles[0].Add(activeTiles[1][0]);
                activeTiles[1].RemoveAt(0);
            }

            for (int tile = 2; tile >= 0; tile--)
            {
                activeTiles[1].Add(activeTiles[2][0]);
                activeTiles[2].RemoveAt(0);
            }

            spawnX = centerTile.position.x - tileLenght;
            spawnZ = centerTile.position.z + tileLenght * 2;

            for (int tempTile = 0; tempTile < amountOfTiles; tempTile++)
            {
                GameObject tile;
                tile = Instantiate(pickRandomTile()) as GameObject;
                tile.transform.SetParent(transform);

                tile.transform.position = new Vector3(spawnX, 0, spawnZ);


                spawnX += tileLenght;

                activeTiles[2].Add(tile);

                //Debug.Log("Tiles in row " +  + ": " + activeTiles[4].Count);
            }


            centerTile = activeTiles[1][1].transform;


        }

        //Move Back
        if (playerTransform.position.z <= (centerTile.transform.position.z - tileLenght))
        {
            DeleteRow(2);

            for (int tile = 2; tile >= 0; tile--)
            {
                activeTiles[2].Add(activeTiles[1][0]);
                activeTiles[1].RemoveAt(0);
            }

            for (int tile = 2; tile >= 0; tile--)
            {
                activeTiles[1].Add(activeTiles[0][0]);
                activeTiles[0].RemoveAt(0);
            }


            spawnX = centerTile.position.x - tileLenght;
            spawnZ = centerTile.position.z - tileLenght * 2;

            for (int tempTile = 0; tempTile < amountOfTiles; tempTile++)
            {
                GameObject tile;
                tile = Instantiate(pickRandomTile()) as GameObject;
                tile.transform.SetParent(transform);

                tile.transform.position = new Vector3(spawnX, 0, spawnZ);


                spawnX += tileLenght;

                activeTiles[0].Add(tile);

                //Debug.Log("Tiles in row " +  + ": " + activeTiles[4].Count);
            }

            centerTile = activeTiles[1][1].transform;






        }

        //Move Right
        if (playerTransform.position.x >= (centerTile.transform.position.x + tileLenght))
        {
            //spawnZ = centerTile.position.z - tileLenght * 2;
            spawnX = centerTile.position.x + tileLenght * 2;
            spawnZ = activeTiles[0][2].transform.position.z;

            for (int row = 0; row <= 2; row++)
            {
                Destroy(activeTiles[row][0]);
                activeTiles[row].RemoveAt(0);
              
              
                GameObject tile;
                tile = Instantiate(pickRandomTile()) as GameObject;
                tile.transform.SetParent(transform);

                tile.transform.position = new Vector3(spawnX, 0, spawnZ);


                spawnZ += tileLenght;

               

                activeTiles[row].Add(tile);
                Debug.Log(activeTiles[row][2]);





            }

            centerTile = activeTiles[1][1].transform;
        }


        //Move Left
        if (playerTransform.position.x <= (centerTile.transform.position.x - tileLenght))
        {
            //spawnZ = centerTile.position.z - tileLenght * 2;
            spawnX = centerTile.position.x - tileLenght * 2;
            spawnZ = activeTiles[0][2].transform.position.z;

            for (int row = 0; row <= 2; row++)
            {
                Destroy(activeTiles[row][2]);
                activeTiles[row].RemoveAt(2);


                GameObject tile;
                tile = Instantiate(pickRandomTile()) as GameObject;
                tile.transform.SetParent(transform);

                tile.transform.position = new Vector3(spawnX, 0, spawnZ);


                spawnZ += tileLenght;



                activeTiles[row].Insert(0,tile);
                Debug.Log(activeTiles[row][2]);





            }

            centerTile = activeTiles[1][1].transform;
        }


    }

    private void DeleteRow(int row)
    {
        

        for (int tile = 2; tile >= 0; tile--) {
            
            Destroy(activeTiles[row][tile]);
            activeTiles[row].RemoveAt(tile);          
            
            Debug.Log("Tile removed at row " + row + ". Tiles left: " + activeTiles[row].Count);
        }




        Debug.Log(activeTiles.ContainsKey(row));

    }

    private GameObject pickRandomTile()
    {
        float R = Random.value * 100;
        float cumulative = 0.0f;
        Debug.Log("R " + R);

        for (int i = 0; i < tilePrefabs.Count; i++)
        {

            cumulative += tilePrefabs[i].GetComponent<TileInfo>().chanceToSpawn;
            Debug.Log(cumulative);
            if (R < cumulative)
                return tilePrefabs[i];
        }

        return null;
    }

    }
