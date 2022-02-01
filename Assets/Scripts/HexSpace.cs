using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Utility class, stores all important information of individual tiles in the grid
[System.Serializable]
public class HexSpace : MonoBehaviour
{

    public enum HexTile { Red, Green, Blue };
    public HexTile hexTile;
    public Vector2 coordinate;
    public Vector3 position;
    public GameObject hexObject;
    public bool interactable;
    //public Item storedItem;
}