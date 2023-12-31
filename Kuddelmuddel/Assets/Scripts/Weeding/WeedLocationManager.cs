using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeedLocationManager : MonoBehaviour
{
    public static WeedLocationManager Instance;
    public Dictionary<Vector3Int, GameObject> weedLocations = new Dictionary<Vector3Int, GameObject>(); // cell, weed reference
    public Dictionary<Vector3Int, GameObject> tileLocations = new Dictionary<Vector3Int, GameObject>();
    
    void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }
}
