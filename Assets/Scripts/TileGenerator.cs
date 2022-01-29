using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TileGenerator : MonoBehaviour {
    public GameObject[] prefabs;
    public Vector2 gridDim;
    public float tileSize;

    public List<HexSpace> hexGridContents = new List<HexSpace>();

    public void Start() {
        GenerateHexGrid(gridDim.x, gridDim.y);
    }
    
    void GenerateHexGrid(float width, float height) {
        float tileHeight = 1.1547f * tileSize; //ratio of hex width to height
        Vector3 gridCenter = new Vector3(-(width * tileSize) / 2 + tileSize, 0, - (height * tileHeight) / 2 + tileHeight);
        Vector3 spawnPos = Vector3.zero;
        int tileCount = 0;


        for (int x = 0; x <= width - 1; x++) {               
            for (int z = 0; z <= height - 1; z++) {
                bool skipPos = false;
                int randIndex = Random.Range(0, prefabs.Length);

                if (z%2 == 0) {
                    if (x != width - 1)
                        spawnPos = new Vector3(x * tileSize, 0, z * tileHeight * .75f) + gridCenter;
                    else skipPos = true;
                } else {                                
                    spawnPos = new Vector3((x * tileSize) - (0.5f * tileSize), 0, z * tileHeight *.75f) + gridCenter;
                }
                if (!skipPos) {
                    GameObject newTile = GameObject.Instantiate(prefabs[randIndex], spawnPos, Quaternion.identity, this.transform);
                    HexSpace newHexSpace = new HexSpace();
                    hexGridContents.Add(newHexSpace);

                    newHexSpace.coordinate = new Vector2(x, z);
                    newHexSpace.position = spawnPos;
                    newHexSpace.hexObject = newTile
                    newHexSpace.hexObject.AddComponent()
                    newHexSpace.hexObject.name = "BlankTile(" + tileCount + ")";
                    newHexSpace.hexObject.transform.localScale = newTile.hexObject.transform.localScale * tileSize;
                    tileCount++;
                }
            }
        }
    }
}

[System.Serializable]
public class HexSpace {

    public enum HexTile { Green, Red, Brown };
    public HexTile hexTile;
    public Vector2 coordinate;
    public Vector3 position;
    public GameObject hexObject;
    //public Item storedItem;
}
