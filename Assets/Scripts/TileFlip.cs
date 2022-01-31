using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Interactable class specific to flipping a tile for the purpose of these placeholder mechanics
[RequireComponent(typeof(Transform))]
[RequireComponent(typeof(Animator))]
public class TileFlip : Interactable {
    //Required components for class to execute
    Transform transform;
	Animator anim;

	public delegate void OnTileFlip(HexSpace space);
	public event OnTileFlip FlipCallback;
	public event OnTileFlip OriginCallback;

	public override void Start() {
		base.Start();

		transform = GetComponent<Transform>();
		anim = GetComponent<Animator>();
	}
//
	public override void Interact() {
		base.Interact();

		StartCoroutine(FlipTile(true));
	}
//Play tile flip animation, trigger adjacent tiles if origin, send events to TileGenerator
	public IEnumerator FlipTile(bool origin) {
		anim.SetBool("Flip", true);
		//yield return new WaitForSeconds(anim.GetCurrentAnimatorClipInfo(0)[0].clip.length);
		//Hardcoded delay bc I couldn't figure out delaying by current animation clip ^^^
		yield return new WaitForSeconds(0.15f);
		anim.SetBool("Flip", false);
		FlipCallback?.Invoke(GetComponent<HexSpace>());

		yield return new WaitForSeconds(0.15f);
		if(origin) OriginCallback?.Invoke(GetComponent<HexSpace>());
	}
//
}
