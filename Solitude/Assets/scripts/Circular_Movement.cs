using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circular_Movement : MonoBehaviour {

	public float radius = 2f;      //Distance from the center of the circle to the edge
	public float currentAngle= 0f; //Our angle, this public for debugging purposes
	private float speed = 0f;      //Rate at which we'll move around the circumference of the circle
	public float timeToCompleteCircle = 1.5f; //Time it takes to complete a full circle
	public bool clockwise =  false; 
	Vector2 speedVector;



	// Use this for initialization
	void Awake () {
		speed = (Mathf.PI * 2) / timeToCompleteCircle;
		speedVector = new Vector2 ();
	}

	// Update is called once per frame
	void Update () {
		speed = (Mathf.PI * 2) / timeToCompleteCircle; 

		currentAngle += Time.deltaTime * speed; //Changes the angle 
		float newX =  radius * 	Mathf.Cos (currentAngle);
		float newY = radius * Mathf.Sin (currentAngle);
		speedVector = new Vector2 (newX, 0f);
//		speedVector = new Vector2 (0.0f, 0.0f);
		if(clockwise){
			this.transform.position = new Vector3 (this.transform.position.x - newX, this.transform.position.y - newY, this.transform.position.z);
		}else{
			this.transform.position = new Vector3 (this.transform.position.x + newX, this.transform.position.y + newY, this.transform.position.z);
		}
	}



	void OnTriggerStay(Collider other) {
		Debug.Log ("The player is on the Circular Platform");
	}

	void OnTriggerExit(Collider other){
		Debug.Log ("The player is leaving the Circular Platform");
	}

	public Vector2 getSpeed(){
		return speedVector;
	}
}
