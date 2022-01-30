using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class OffsetOnHover : MonoBehaviour {

	public float yOffset = 0.25f;
	public float duration = 0.125f;
	
	float yOrigin;

	void Start() {
		yOrigin = transform.position.y;
	}

	public IEnumerator Activate() {
		float time = 0;
		while(time < duration) {
			//float t = time / duration;
			//t = t * t * (3f - 2f * t);		
			transform.position = 
				new Vector3(
				transform.position.x, 
				Mathf.Lerp(yOrigin, yOrigin + yOffset, time/duration), 
				transform.position.z);
			time += Time.deltaTime;
			yield return null;
		}	
	}

	public IEnumerator Deactivate() {
		float time = 0;
		while(time <= duration) {			
			//float t = time / duration;
			//t = t * t * (3f - 2f * t);		
			transform.position = 
				new Vector3(
				transform.position.x, 
				Mathf.Lerp(transform.position.y, yOrigin, time/duration), 
				transform.position.z);
			time += Time.deltaTime;
			yield return null;
		}	
	}
}
