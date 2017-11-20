using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallaxing : MonoBehaviour {

	public Transform[] backgrounds;
	private float[] parallaxScales;
	public float smoothing = 1f;

	private Transform cam;
	private Vector3 previousCamPos;


	void Awake(){
		//set up reference variables
		cam = Camera.main.transform;
	}
	// Use this for initialization
	void Start () {
		//the previous had the current frame's camera position
		previousCamPos =  cam.position;
		parallaxScales = new float[backgrounds.Length];
		for (int i = 0; i < backgrounds.Length; i++) {
			parallaxScales [i] = backgrounds [i].position.z * -1; 
		}

		
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < backgrounds.Length; i++) {
			//the parallax is the opposite of the camera movement because he previous frame multiplied by the scale 
			float parallex  = (previousCamPos.x - cam.position.x) * parallaxScales[i];

			//set a target x position which is current position plus the parallax
			float backgroundTargetPosX  = backgrounds[i].position.x + parallex;

			//CREATE a target position which is the background current position with it's target X position 
			Vector3 backgroundTargetPos = new Vector3(backgroundTargetPosX , backgrounds[i].position.y,backgrounds[i].position.z);

			backgrounds [i].position = Vector3.Lerp (backgrounds [i].position, backgroundTargetPos, smoothing * Time.deltaTime);
		}
		//set the previousCamPOs to the camera's position at the end of the frame 
		previousCamPos = cam.position;
	}
}
