using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Really just throwing things together here tbh
public class Announcements : MonoBehaviour {

    TileGenerator tileGen;
    TotalsDisplay totals;

    public GameObject[] barkPanels; //BarkPanel prefabs, different announcments

    void Start() {
        tileGen = TileGenerator.instance;
        tileGen.GridGeneratedCallback += OnGridGenerated;
        totals = TotalsDisplay.instance;
        totals.OnWinCallback += OnWin;
        //totals.OnLoseCallback += OnLose;
        foreach (GameObject go in barkPanels) {
            go.SetActive(true);
        }
    }

    void OnGridGenerated(float r, float g, float b) {//Passing these parameters just to avoid making a new event :\
        StartCoroutine(Bark(0));    
    }

    void OnWin() {
        StartCoroutine(Bark(1));
    }

    void OnLose() {
        StartCoroutine(Bark(2));
    }
    
    //play bark animation at index
    public IEnumerator Bark(int index) {
        barkPanels[index].GetComponent<Animator>().SetBool("Bark", true);
        yield return new WaitForSeconds(1);
        barkPanels[index].GetComponent<Animator>().SetBool("Bark", false);
    }

}
