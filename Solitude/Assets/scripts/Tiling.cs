using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(SpriteRenderer))]
public class Tilling : MonoBehaviour {


	public int offsetX = 2; //The offset so that we dont get any weird errors

	//these are used for checking if we need to instantiate stuff
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;
	public bool reverseScale = false;


	//used if the object is not tileable 
	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform ;


	void Awake(){
		cam = Camera.main;
		myTransform = transform;

	}
	// Use this for initialization
	void Start () {
		SpriteRenderer sRenderer = GetComponent<SpriteRenderer> ();
		spriteWidth = sRenderer.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		//does is still need buddies 
		if(!hasALeftBuddy || !hasARightBuddy){
			//get the distance between the center of the camera towards its edge (half the width ) in world coordinates  
			float camHorizontalExtend = cam.orthographicSize * Screen.width / Screen.height;

			//calculate the X position where the camera can see the edge of the sprite
			float edgeVisiblePosRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePosLeft = (myTransform.position.x - spriteWidth / 2) + camHorizontalExtend;

			//checking if we can the edge of the element and then calling makeNewBuddy if we can 
			if (cam.transform.position.x >= edgeVisiblePosRight - offsetX && hasARightBuddy == false) {
				makeNewBuddy (1);
			} else if (cam.transform.position.x <= edgeVisiblePosLeft + offsetX && hasALeftBuddy == false) {
				makeNewBuddy (-1);
			}
		}
	}


	void makeNewBuddy(int direction){
		//calculatig the new position for new buddy 
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * direction, myTransform.position.y, myTransform.position.z);
	}
}
