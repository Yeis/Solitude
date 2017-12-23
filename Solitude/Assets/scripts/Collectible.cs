using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Collectible : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerStay2D(Collider2D other) {
		if(this.gameObject.name == "Recuerdo_Transportador_1"){
			SceneManager.LoadScene("inHell");
		}

		Debug.Log ("The player is on the collectible");
	}

	void OnTriggerExit2D(Collider2D other){
		Debug.Log ("The player is leaving");
	}
}
