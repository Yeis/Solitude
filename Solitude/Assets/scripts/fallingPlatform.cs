using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatform : MonoBehaviour {

	public float timeToFall = 2.0f; //How much time until the platform falls 
	public float maxGravityScale = 1.0f; //maximum amount of gravity that we want 
	private bool onPlatform = false; //boolean that handle if the player is on the platform 
	private Rigidbody2D platformRigidBody; //rigidbody Component of the platform


	void Awake () {
		platformRigidBody = GetComponent<Rigidbody2D>();
	}
	void OnTriggerStay2D(Collider2D other) {
		onPlatform = true;
		Debug.Log ("The player is on the falling cloud");
	} 
	// Update is called once per frame
	void Update () {
		if(onPlatform){
			timeToFall -= Time.deltaTime;
			if(timeToFall < 0  && platformRigidBody.gravityScale < maxGravitySpeed){
				platformRigidBody.gravityScale +=  Time.deltaTime;
			}
	}

	
	}
}
