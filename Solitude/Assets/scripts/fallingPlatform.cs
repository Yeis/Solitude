using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fallingPlatform : MonoBehaviour {

	public float timeToFall = 2.0f;
	private bool onPlatform = false;
	public float maxGravitySpeed = 1.0f;

	Rigidbody2D platformRigidBody;

		void OnTriggerStay2D(Collider2D other) {
		onPlatform = true;
		Debug.Log ("The player is on the falling cloud");
	} 
	// Use this for initialization
	void Awake () {
		platformRigidBody = GetComponent<Rigidbody2D>();
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
