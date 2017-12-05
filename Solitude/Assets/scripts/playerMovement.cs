using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {


	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "movement_cloud") {
			Vector2 speed = col.gameObject.GetComponent<nube_movimiento> ().getSpeed ();
			Vector2 gameObjectPosition = this.gameObject.transform.position;
			Debug.Log ("Detecting Simple Cloud with Speed: X: " + speed.x + "Y:  " + speed.y);
			this.gameObject.transform.position = new Vector2 (gameObjectPosition.x + (speed.x * Time.deltaTime), gameObjectPosition.y + (speed.y * Time.deltaTime));
		} else if (col.gameObject.tag == "circular_cloud") {
			Vector2 speed = col.gameObject.GetComponent<Circular_Movement> ().getSpeed ();
			Vector2 gameObjectPosition = this.gameObject.transform.position;
			Debug.Log ("Detecting Circular Cloud with Speed: X: " + speed.x + "Y:  " + speed.y);
			this.gameObject.transform.position = new Vector2 (gameObjectPosition.x + (speed.x * Time.deltaTime), gameObjectPosition.y + (speed.y * Time.deltaTime));
		} else if (col.gameObject.tag == "collectible") {
			Destroy(col.gameObject);
		}

	}
	void OnTriggerExit2D(Collider2D col){
	}

}	