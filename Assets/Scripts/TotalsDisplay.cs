using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//Quick UI script for placeholder game mechanics
public class TotalsDisplay : MonoBehaviour {
 //Define variables used to display UI and store values
	public TileGenerator tileGen;

	public Slider[] sliders;
	public Text[] valuesText;
	public Text[] goalsText;

	int[] values = new int[3];
	int[] goals = new int[3];

	public delegate void OnEndCondition();
	public event OnEndCondition OnWinCallback;
	public event OnEndCondition OnLoseCallback;

	public static TotalsDisplay instance;
	void Awake() { 
		if (instance != null) { 
			Debug.Log("More than one instance of TotalsDisplay found!");
    		return;
    	}
    	instance = this;
	}

//
	void Start() {
		//Grab instance ref for event callbacks to update UI from TileGenerator
		tileGen = TileGenerator.instance;
		tileGen.GridCalculatedCallback += UpdateGoals;
		tileGen.TotalsChangeCallback += UpdateTotals;
	}
//Update the totals required to win in the UI when grid is calculated
	void UpdateGoals(float r, float g, float b) {
		int totalTiles = (int)(tileGen.gridDim.x * tileGen.gridDim.y);
		goals[0] = (int)(totalTiles * r);
		goals[1] = (int)(totalTiles * g);
		goals[2] = (int)(totalTiles * b);

		UpdateDisplay();
	}
//
//Update current values stored in each colors slider each time a hex is flipped in TileGenerator
	void UpdateTotals(float r, float g, float b) {
		values[0] = (int)r;
		values[1] = (int)g;
		values[2] = (int)b;

		UpdateDisplay();
		if (values[0] >= goals[0] && values[1] >= goals[1] && values[2] >= goals[2]) OnWinCallback?.Invoke();
	}
//
//Update all fields of the UI elements which display win conditions
	void UpdateDisplay() {
		for (int i = 0; i < 3; i++) {
			sliders[i].maxValue = goals[i];
			sliders[i].value = values[i];
			goalsText[i].text = "/" + goals[i].ToString();
			valuesText[i].text = values[i].ToString();
		}

	}

}
