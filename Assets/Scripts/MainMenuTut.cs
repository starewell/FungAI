using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuTut : MonoBehaviour {

    public TileFlip tile;

    void Start() {
        tile.FlipCallback += FlipTile;
    }

    public void FlipTile(HexSpace space) { 
        

    }

}
