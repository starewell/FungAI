using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This class is really bloated and messy bc I've just been jamming, I have ideas of how to clean it up and section into new classes
public class TileGenerator : MonoBehaviour {

//Definitions for the grid fabrication
    //These will be defined with a level editor class in the future
    public GameObject[] prefabs;
    public Material[] colors;
    public Vector2 gridDim; //Odd y values generate rounded grid shapes
    public float tileSize;
    //% of grid for each tile type, procgen variables and game manager
    [Range(0,1)]
    public float spawnPercentRed;
    [Range(0,1)]
    public float spawnPercentGreen;
    [Range(0,1)]
    public float spawnPercentBlue;
	[Range(0,1)]
    public float goalPercentRed;
    [Range(0,1)]
    public float goalPercentGreen;
    [Range(0,1)]
    public float goalPercentBlue;
    //
    public List<HexSpace> hexGridContents = new List<HexSpace>();  //Stored list of all tiles in grid; HexSpaces

    public delegate void OnGridChange(float r, float g, float b);
    public event OnGridChange GridCalculatedCallback;
	public event OnGridChange TotalsChangeCallback;

    //Picked this up a while ago, use it liberally but would like to be more
    public static TileGenerator instance;
    void Awake() {
    	if (instance != null) {
    		Debug.Log("More than one instance of TileGenerator found!");
    		return;
    	}
    	instance = this;
    }
    //
    Announcements barks;
    public void Start() {
        barks = Announcements.instance;

        StartCoroutine(GenerateHexGrid(gridDim.x, gridDim.y));
    }

//Primary function of class, create a grid of hexes and store their important data in a centralized list     
    IEnumerator GenerateHexGrid(float gridWidth, float gridHeight) {
    //Defining local variables used in function
        float tileHeight = tileSize * 1.1547f; //Width multiplied by ratio of hex width to height
        float zOffset = tileHeight * 0.75f; //Height multiplied by 3/4 -- height array ratio of hex grids
        Vector3 gridCenter = new Vector3(-(gridWidth * tileSize) / 2 + tileSize, 0, - (gridHeight * tileHeight) / 2 + tileHeight); //Used to offset spawnPos, centers grid to scene origin
        Vector3 spawnPos = Vector3.zero; //Defualt zeroed definition

        int[] tilePools = DistributeTilesToPools(spawnPercentRed, spawnPercentGreen, spawnPercentBlue, gridWidth, gridHeight); //Setup for procgen
        GridCalculatedCallback?.Invoke(goalPercentRed, goalPercentGreen, goalPercentBlue); //Event for UI class TotalsDisplay
    //
    //Core loop, cycles through dimensions defined in inspector (level editor class)
        for (int x = 0; x <= gridWidth - 1; x++) {               
            for (int z = 0; z <= gridHeight - 1; z++) {
                bool skipPos = false; //Defined to reference valid spawn positions

            //Placeholder procgen of tile type -- hardly works w/ even distribution
                HexSpace.HexTile newTileType = ProcGenDistrubtion(tilePools);
            //
            //Checking and defining valid spawn positions             
                if (z%2 == 0) { //Even rows                   
                    if (x != gridWidth - 1) //Checks if the last column, skips even rows for a rounded grid shape
                        spawnPos = new Vector3(x * tileSize, 0, z * zOffset) + gridCenter;
                    else skipPos = true;
                } else { //Odd rows -- needs different x offset                           
                    spawnPos = new Vector3((x * tileSize) - (0.5f * tileSize), 0, z * zOffset) + gridCenter;
                }
            //
            //Instantiation of tile gameobject, definition of HexSpace and append to list
                if (!skipPos) {                                            
                    GameObject newTile = GameObject.Instantiate(prefabs[0], spawnPos, Quaternion.identity, this.transform);                    
                    newTile.AddComponent<HexSpace>();              
                    HexSpace newHexSpace = newTile.GetComponent<HexSpace>();    
                    //Gosh I love the bottleneck of storing data, it all comes together here
                    UpdateHexSpace(newHexSpace, newTileType, colors[(int)newTileType], new Vector2(x,z), spawnPos, newTile, false);

                    hexGridContents.Add(newHexSpace);
                    newHexSpace.GetComponent<TileFlip>().FlipCallback += FlipHexSpace;
                    newHexSpace.GetComponent<TileFlip>().OriginCallback += FlipAdjacent;

                    newTile.name = newTileType.ToString() + "Tile(" + x + ", " + z + ")";

                    UpdateHexGridTotals();

                    yield return new WaitForSeconds((1 + (gridWidth * gridHeight) / 85) / (gridWidth * gridHeight)); // fun hardcoded time calculation -- dependent on grid size                
                }
            //
            }
        //
        }
        //Call announcements and unlock tiles so the game begins!
        StartCoroutine(barks.Bark(0));
        yield return new WaitForSeconds(1);       
        LockHexGrid(false);
    }
//Utility function, selects tiles randomly between defined pools
//Does not work... will want to revise 
    HexSpace.HexTile ProcGenDistrubtion(int[] pools) {
        int tileIndex = -1;
        bool[] remaining = new bool[]{true, true};

        if (Random.Range(0, 1) == 0 && pools[3] > 0) {
            tileIndex = Random.Range(0,3);
            pools[3]--;
        } else {
            int range = 0;
            if (pools[0] > 0) range++; else remaining[0] = false;
            if (pools[1] > 0) range++;  else remaining[1] = false;
            if (pools[2] > 0) range++;
            tileIndex = Random.Range(0, range);
            if(!remaining[0]) tileIndex+=1;
            if(!remaining[1] && tileIndex != 0) tileIndex+=1;
            pools[tileIndex]--;            
        }

        switch(tileIndex) {
            default:
            return HexSpace.HexTile.Blue;
            case 0:
            return HexSpace.HexTile.Red;
            case 1:
            return HexSpace.HexTile.Green;
            case 2:
            return HexSpace.HexTile.Blue;
        }
    }
//Setup for procgen, distributes total tiles to percentages of each color and random
//This function breaks if the inspector fields are filled greater than 100% distribution, is there a way to restrict the float variables so they don't exceed 1 when combined?
    static int[] DistributeTilesToPools(float redP, float greenP, float BlueP, float gridWidth, float gridHeight) {
        int totalTiles = (int)((gridWidth * gridHeight) - gridHeight / 2);
        int[] pools = new int[4];

        pools[0] = (int)(totalTiles * redP);
        pools[1] = (int)(totalTiles * greenP);
        pools[2] = (int)(totalTiles * BlueP);
        pools[3] = totalTiles - pools[0] - pools[1] - pools[2];
        //Debug.Log(totalTiles + " TotalTiles, " + pools[0] + " RedTiles, " + pools[1] + " GreenTiles, " + pools[2] + " BlueTiles, " + pools[3] + " RandomTiles");
        return pools;
    }
//
//Function is executed through delegate event from the TileFlip class
    void FlipHexSpace(HexSpace space) {
    	int index = (int)space.hexTile + 1;
    	if (index > colors.Length - 1) index = 0;

    	space.hexObject.GetComponent<Renderer>().material = colors[index];
    	space.hexTile = (HexSpace.HexTile)index;
        space.hexObject.name = space.hexTile.ToString() + "Tile(" + space.coordinate.x + ", " + space.coordinate.y + ")";

        UpdateHexGridTotals();
    }
//
//Function is executed through delegate event from the TileFlip class
    void FlipAdjacent(HexSpace space) {
    	Vector2 originCoord = space.coordinate;
    	Vector2[] adjacentCoord;
    	if (originCoord.y%2 == 0) { //Even rows
    		adjacentCoord = new Vector2[] {
	    	new Vector2(originCoord.x, originCoord.y-1),
	    	new Vector2(originCoord.x-1, originCoord.y),
	    	new Vector2(originCoord.x, originCoord.y+1),
	    	new Vector2(originCoord.x+1, originCoord.y-1),
	    	new Vector2(originCoord.x+1, originCoord.y+1),
	    	new Vector2(originCoord.x+1, originCoord.y)};
    	} else { //Odd rows
    		adjacentCoord = new Vector2[] {
	    	new Vector2(originCoord.x-1, originCoord.y-1),
	    	new Vector2(originCoord.x-1, originCoord.y),
	    	new Vector2(originCoord.x-1, originCoord.y+1),
	    	new Vector2(originCoord.x, originCoord.y-1),
	    	new Vector2(originCoord.x, originCoord.y+1),
	    	new Vector2(originCoord.x+1, originCoord.y)};
    	}
    	foreach(Vector2 coord in adjacentCoord) {
    		if(hexGridContents.Find(space => space.coordinate == coord) != null) { //Found this constructor thru microsoft docs, have never used it before
    			HexSpace adjSpace = hexGridContents.Find(space => space.coordinate == coord);    		
    			StartCoroutine(adjSpace.GetComponent<TileFlip>().FlipTile(false));
    		}
    	}

    }
//
//Utility function, updates all fields of the utility class HexSpace
    void UpdateHexSpace(HexSpace space, HexSpace.HexTile tile, Material mat, Vector2 coord, Vector3 pos, GameObject go, bool active) {        
        space.hexTile = tile;
        space.coordinate = coord;
        space.position = pos;
        space.hexObject = go;
        space.hexObject.GetComponent<Renderer>().material = mat;
        space.interactable = active;       
    }
//

    void LockHexGrid(bool locked) {
    	if(locked) {
    		foreach(HexSpace space in hexGridContents) {
    			space.interactable = false;
    		}
    	} else {
    		foreach(HexSpace space in hexGridContents) {
    			space.interactable = true;
    		}
    	}
    }
//Temporarily store grid totals, call for UI to refresh after tiles flip
    void UpdateHexGridTotals() {
    	int amntRed = 0, amntGreen = 0, amntBlue = 0;
    	foreach (HexSpace space in hexGridContents) {
    		if ((int)space.hexTile == 0) amntRed++;
    		else if ((int)space.hexTile == 1) amntGreen++;
    		else if ((int)space.hexTile == 2) amntBlue++;
    	}

    	TotalsChangeCallback?.Invoke(amntRed, amntGreen, amntBlue);
    }
}

//Utility class, stores all important information of individual tiles in the grid
[System.Serializable]
public class HexSpace : MonoBehaviour {

    public enum HexTile { Red, Green, Blue };
    public HexTile hexTile;
    public Vector2 coordinate;
    public Vector3 position;
    public GameObject hexObject;
    public bool interactable;
    //public Item storedItem;
}