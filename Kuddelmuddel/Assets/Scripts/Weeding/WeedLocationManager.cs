using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeedLocationManager : MonoBehaviour
{
    public Dictionary<Vector3Int, GameObject> weedLocations = new Dictionary<Vector3Int, GameObject>(); // cell, weed reference
    public Dictionary<Vector3Int, GameObject> tileLocations = new Dictionary<Vector3Int, GameObject>();
    
    public int GetNumWeeds() {
        int count = 0;
        foreach (KeyValuePair<Vector3Int, GameObject> entry in weedLocations) {
            if (entry.Value.tag == "Weed"){
                count++;
            }
        }
        return count;
    }

}
