using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeedPlanter : MonoBehaviour
{
    [SerializeField] public GameObject weedPrefab;
    [SerializeField] private Tilemap canvas;
    
    private WeedLocationManager wlm;
    private TileGetter tg;
    private PlayerData pd;

    void Start() {
        tg = GameObject.Find("Touch Manager").GetComponent<TileGetter>();
        wlm = GameObject.Find("Weed Location Manager").GetComponent<WeedLocationManager>();
        pd = GameObject.Find("Player").GetComponent<PlayerData>();
    }

    public void PlanterUpdate() {
        if (Input.touchCount > 0){
            tg.TouchUpdate(canvas, Input.GetTouch(0).position);

            if (!(wlm.weedLocations.ContainsKey(tg.lastCell))){
                if (wlm.tileLocations.ContainsKey(tg.lastCell)){
                    if (pd.seedCount > 0){
                        GameObject newWeed = Instantiate(weedPrefab, canvas.CellToWorld(tg.lastCell), Quaternion.identity);
                        newWeed.transform.parent = UnityEngine.GameObject.Find("Above Ground").transform;
                        newWeed.name = "Weed" + tg.lastCell;
                        newWeed.GetComponent<WeedData>().testNum = wlm.weedLocations.Count;
                        pd.seedCount -= 1;
                        print("Added weed at " + tg.lastCell);
                        wlm.weedLocations.Add(tg.lastCell, newWeed);
                    }
                    else{
                        print("You don't have enough seeds to plant a weed!");
                    }
                }
                else{
                    print("There is no tile to place that weed!");
                }
            }
            else if (Input.GetTouch(0).phase == TouchPhase.Began){
                print("A weed already exists at " + tg.lastCell);
            }
        }
    }
}