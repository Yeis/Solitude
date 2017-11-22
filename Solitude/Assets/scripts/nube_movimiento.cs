using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class nube_movimiento : MonoBehaviour {


	public Vector3 pointB;
	public float time =  3.0f;
	Vector2 Speed;


	IEnumerator Start(){
		//Initial Setup
		Vector3 pointA = transform.position;
		if (pointA.x > pointB.x) {
			Speed = (pointA - pointB) / time;
			Speed.x *= -1;
		} else {
			Speed = (pointB - pointA) / time;

		}
		while (true) {
			yield return  StartCoroutine(MoveObject(transform ,pointA , pointB , time));
			Speed.x *= -1;
			yield return  StartCoroutine(MoveObject(transform ,pointB , pointA , time));
			Speed.x *= -1;

		}
	}

	IEnumerator MoveObject(Transform cloudTransform  , Vector3 startPos , Vector3 endPos ,  float time){
		float i = 0.0f;
		float rate = 1.0f / time;
		while (i < 1.0f) {
			i += Time.deltaTime * rate;
			cloudTransform.position = Vector3.Lerp (startPos, endPos, i);
			yield return null;
		}
	}


	void OnTriggerStay(Collider other) {
		Debug.Log ("The player is on the platfoem");
	}

	void OnTriggerExit(Collider other){
		Debug.Log ("The player is leaving");
	}


	public Vector2 getSpeed(){
		return Speed;
	}
}
