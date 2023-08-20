using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeedUserTemplate : MonoBehaviour
{
    private WeedLocationManager wlm;
    private TileGetter tg;

    void Start(){
        tg  = GameObject.Find("Touch Manager").GetComponent<TileGetter>();
        wlm = GameObject.Find("Weed Location Manager").GetComponent<WeedLocationManager>();
    }

    void Update()
    {
        if (Input.touchCount > 0) {
            Touch touch = Input.GetTouch(0);

            tg.TouchUpdate(touch.position);

            if (wlm.weedLocations.ContainsKey(tg.lastCell) && wlm.weedLocations[tg.lastCell].tag == "Weed" && touch.phase == TouchPhase.Began) {
                GameObject clickedWeed = wlm.weedLocations[tg.lastCell];
                WeedData data = clickedWeed.GetComponent<WeedData>();

                // // Read from WeedData.cs
                print("You touched weed at " + data.location);
                
                // Write to WeedData.cs:
                //data.testString = something something

                // WeedData is used for custom properties (age, type of weed)
                // GameObject properties (location, sprite) modifiable as well
            }
        }
    }
}