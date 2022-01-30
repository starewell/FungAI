using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TotalsDisplay : MonoBehaviour {
  
	public TileGenerator tileGen;

	public Slider[] sliders;
	public Text[] valuesText;
	public Text[] goalsText;

	int[] values = new int[3];
	int[] goals = new int[3];

	void Start() {
		tileGen = TileGenerator.instance;
		tileGen.GridGeneratedCallback += UpdateGoals;
		tileGen.TotalsChangeCallback += UpdateTotals;
	}

	void UpdateGoals(float r, float g, float b) {
		int totalTiles = (int)(tileGen.gridDim.x * tileGen.gridDim.y);
		goals[0] = (int)(totalTiles * r);
		goals[1] = (int)(totalTiles * g);
		goals[2] = (int)(totalTiles * b);

		UpdateDisplay();
	}

	void UpdateTotals(float r, float g, float b) {
		values[0] = (int)r;
		values[1] = (int)g;
		values[2] = (int)b;
		UpdateDisplay();
	}

	void UpdateDisplay() {
		for (int i = 0; i < 3; i++) {
			sliders[i].maxValue = goals[i];
			sliders[i].value = values[i];
			goalsText[i].text = "/" + goals[i].ToString();
			valuesText[i].text = values[i].ToString();
		}

	}

}
