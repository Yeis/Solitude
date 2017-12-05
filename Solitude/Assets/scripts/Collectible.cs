using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		Debug.Log ("The player is on the platfoem");
	}

	void OnTriggerExit2D(Collider2D other){
		Debug.Log ("The player is leaving");
	}
}
