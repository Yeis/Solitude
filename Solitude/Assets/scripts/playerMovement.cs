using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {


	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject.tag == "movement_cloud") {
			Vector3 speed = col.gameObject.GetComponent<nube_movimiento> ().getSpeed ();
			Vector2 gameObjectPosition = this.gameObject.transform.position;
			this.gameObject.transform.position = new Vector2 (gameObjectPosition.x + (speed.x * Time.deltaTime), gameObjectPosition.y + (speed.y * Time.deltaTime));


		}

	}
	void OnTriggerExit2D(Collider2D col){
	}

}	