using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
 
	public Camera cam;
	public LayerMask interactionMask;

	void Start() {
		if(cam == null) {
			if (GetComponent<Camera>() != null) {
				cam = GetComponent<Camera>();
			}
		}
	}

	void Update() {
	    if(Input.GetMouseButtonDown(0)) {
			Ray ray = cam.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (Physics.Raycast(ray, out hit, 100f, interactionMask)) {
				hit.collider.GetComponent<Interactable>().OnClicked();
			}
		}
	}

}
