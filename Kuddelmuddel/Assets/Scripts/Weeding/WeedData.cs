using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class WeedData : MonoBehaviour
{
    private TileGetter tg;
    private WeedPlanter wp;
    [SerializeField] private Sprite seedSprite;
    [SerializeField] private Sprite weedSprite;

    public int growthState = 0; //used to determine which animation to display?
    public float growthRate = 10f;
    public float spreadRate = 5f; // seconds
    public float spreadChance = 0.75f;
    public int newWeedsPerSpread = 1;
    public int weedSellValue = 1; // every x sold gets 1 seed back
    public int health = 100;
    public bool canSpread = true;
    public bool isGrown = false;
    public bool isGrowing = false;
    public bool isDamagable = true;
    public float totalGrowth = 0; //incremental growth 0 - 100. Grows 5% a second at growth rate of 100? 5 = 5%
    public Vector3Int location;

    void Start() {
        tg = GameObject.Find("Touch Manager").GetComponent<TileGetter>();
        wp = GameObject.Find("Touch Manager").GetComponent<WeedPlanter>();
    }

    public IEnumerator SpreadLoop() {
        print("started spreading loop");
        while (canSpread) {
            yield return new WaitForSeconds(spreadRate);

            List <Vector3Int> freeCells = tg.GetSurroundingFreeCells(location);
            float spreadCheck = Random.Range(0f,1f);
            if (freeCells.Count > 0 && spreadCheck <= spreadChance){
                for (int i = 0; i < newWeedsPerSpread; i++) {
                    Vector3Int newCell = freeCells[Random.Range(0, freeCells.Count)];
                    wp.CreateWeed(newCell);
                }
            }
            canSpread = tg.GetSurroundingFreeCells(location).Count > 0;
        }
    }

    public IEnumerator GrowingStage() {
        isGrowing = true;
        yield return new WaitForSeconds(growthRate);
        GrowToNextStage();
    }

    public void GrowToNextStage() {
        print("grown!");
        isGrowing = false;
        isGrown = true;
        weedSellValue = 2;
        this.gameObject.GetComponent<SpriteRenderer>().sprite = seedSprite;
        StartCoroutine(SpreadLoop());
    }

}
