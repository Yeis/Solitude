using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class playerMovement : MonoBehaviour {


	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "movement_cloud") {
			this.gameObject.transform.parent = col.gameObject.transform;
			// Vector2 speed = col.gameObject.GetComponent<nube_movimiento> ().getSpeed ();
			// Vector2 gameObjectPosition = this.gameObject.transform.position;
			// Debug.Log ("Detecting Simple Cloud with Speed: X: " + speed.x + "Y:  " + speed.y);
			// this.gameObject.transform.position = new Vector2 (gameObjectPosition.x + (speed.x * Time.deltaTime), gameObjectPosition.y + (speed.y * Time.deltaTime));
		} else if (col.gameObject.tag == "circular_cloud") {
			this.gameObject.transform.parent = col.gameObject.transform;
			// Vector2 speed = col.gameObject.GetComponent<Circular_Movement> ().getSpeed ();
			// Vector2 gameObjectPosition = this.gameObject.transform.position;
			// Debug.Log ("Detecting Circular Cloud with Speed: X: " + speed.x + "Y:  " + speed.y);
			// this.gameObject.transform.position = new Vector2 (gameObjectPosition.x + (speed.x * Time.deltaTime), gameObjectPosition.y + (speed.y * Time.deltaTime));
		} else if (col.gameObject.tag == "collectible") {
			Destroy(col.gameObject);
		} else if(col.gameObject.tag == "boundary"){
			SceneManager.LoadScene("inHeaven");
		}
	}
	void OnTriggerExit2D(Collider2D col){
		this.gameObject.transform.parent = null;
	}

}	