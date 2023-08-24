using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.Events;

public class MapManager : MonoBehaviour
{
    private GameObject tilePrefab;
    private GameObject obstaclePrefab;
    public UnityEvent MapFilled;
    [SerializeField] private Tilemap canvas;
    [SerializeField] private Sprite[] sprites;

    void Start() {
        // Instantiate helper classes
        tilePrefab = GameObject.Find("TileObject");
        obstaclePrefab = GameObject.Find("Obstacle");
        MakeRandomGrid();
        SpawnObstacles();

        // Add event listeners
        MapFilled.AddListener(PlayerData.Instance.SetNextProgression);
        MapFilled.AddListener(MakeRandomGrid);
        MapFilled.AddListener(SpawnObstacles);
    }

    void MakeRandomGrid() { // random procedural generator
        for (int i = PlayerData.Instance.xBounds; i > -PlayerData.Instance.xBounds; i--){
            for (int j = PlayerData.Instance.yBounds; j > -PlayerData.Instance.yBounds; j--){
                Vector3Int cell = new Vector3Int(i,j,0);

                if (!WeedLocationManager.Instance.tileLocations.ContainsKey(cell)){
                    GameObject newTile = Instantiate(tilePrefab, canvas.CellToWorld(cell), Quaternion.identity);
                    newTile.transform.parent = GameObject.Find("Terrain").transform;
                    newTile.name = "Tile (" + i + ", " + j + ", 0)";
                    WeedLocationManager.Instance.tileLocations.Add(cell, newTile);
                    
                    // Random sprite
                    int choice = Random.Range(0,2);
                    newTile.GetComponent<SpriteRenderer>().sprite = sprites[choice];
                }
            }
        }
    }

    void SpawnObstacles() {
        while (PlayerData.Instance.numObstaclesToSpawn > 0) {
            int x = Random.Range(-PlayerData.Instance.xBounds+1, PlayerData.Instance.xBounds);
            int y = Random.Range(-PlayerData.Instance.yBounds+1, PlayerData.Instance.yBounds);
            Vector3Int cell = new Vector3Int(x, y, 0);

            if (!WeedLocationManager.Instance.weedLocations.ContainsKey(cell)) {
                GameObject newObstacle = Instantiate(obstaclePrefab, canvas.CellToWorld(cell), Quaternion.identity);
                newObstacle.transform.parent = GameObject.Find("Above Ground").transform;
                newObstacle.name = "Obstacle (" + x + ", " + y + ", 0)";
                newObstacle.GetComponent<ObstacleData>().location = cell;
                WeedLocationManager.Instance.weedLocations.Add(cell, newObstacle);
                PlayerData.Instance.numObstaclesToSpawn--;
            }
        }
    }

    public void CheckMap() {
        // Progress map if all weeds placed
        bool isMapFilled = true;
        for (int i = PlayerData.Instance.xBounds; i > -PlayerData.Instance.xBounds; i--){
            for (int j = PlayerData.Instance.yBounds; j > -PlayerData.Instance.yBounds; j--){

                Vector3Int cell = new Vector3Int(i,j,0);
                if (!WeedLocationManager.Instance.weedLocations.ContainsKey(cell) || // weed doesn't exist
                    WeedLocationManager.Instance.weedLocations[cell].tag != "Weed" || // obstacle exists
                    !WeedLocationManager.Instance.weedLocations[cell].GetComponent<WeedData>().isGrown){ // weed isn't fully grown
                    isMapFilled = false;
                }
            }
        }

        if (isMapFilled){
            MapFilled.Invoke();
            print("Map filled!");
        }
    }
}
