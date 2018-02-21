using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : RayCastController {

	public LayerMask passengerMask;
	//VECTOR TO MOVE JUST ALONG ONE AXIS 
	// public Vector3 move;

	//waypoints
	public Vector3[] localWaypoints;
	Vector3[] globalWaypoints; 
	public float speed;
	int fromWaypointIndex;
	float percentBetweenwaypoints;

	[Range(0,3)]
	public float easeAmount;
	public bool cyclicWaypoints;
	public float waitTime;
	float nextMoveTime;


	List<PassengerMovement> passengerMovement;
	Dictionary<Transform,Controller2D> passengerDictionary =  new Dictionary<Transform, Controller2D>();
	// Use this for initialization
	public  override void  Start () {
		base.Start();

		globalWaypoints = new Vector3[localWaypoints.Length];
		for (int i = 0; i < localWaypoints.Length; i++){
			globalWaypoints[i] = localWaypoints[i] + transform.position;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
		UpdateRaycastOrigins();
		//Vector3 velocity = move * Time.deltaTime;
		Vector3 velocity = CalulatePlatformMovement();
		CalculatePassengerMovement(velocity);
		MovePassengers(true);
		transform.Translate(velocity);
		MovePassengers(false);
		
	}

	float Ease(float x){
		float a = easeAmount + 1;
		return Mathf.Pow(x,a) / (Mathf.Pow(x,a) + Mathf.Pow(1 - x , a));
	}

	Vector3 CalulatePlatformMovement(){
		if(Time.time < nextMoveTime){
			return Vector3.zero;
		}
		fromWaypointIndex %= globalWaypoints.Length;
		int toWaypointIndex = (fromWaypointIndex + 1) % globalWaypoints.Length;
		float distanceBetweenWaypoints = Vector3.Distance(globalWaypoints[fromWaypointIndex],globalWaypoints[toWaypointIndex]);
		percentBetweenwaypoints += Time.deltaTime * speed / distanceBetweenWaypoints;
		percentBetweenwaypoints = Mathf.Clamp01(percentBetweenwaypoints);
		float eastPercentBetweenEndpoints = Ease(percentBetweenwaypoints);
		Vector3 newPos = Vector3.Lerp(globalWaypoints[fromWaypointIndex],globalWaypoints[toWaypointIndex],eastPercentBetweenEndpoints);
		if(percentBetweenwaypoints >=1){
			percentBetweenwaypoints = 0;
			fromWaypointIndex++;
			if(!cyclicWaypoints){
				if(fromWaypointIndex >= globalWaypoints.Length-1){
					fromWaypointIndex = 0;
					System.Array.Reverse(globalWaypoints);
				}
			}
			nextMoveTime = Time.time + waitTime;
		}
		return newPos - transform.position;
	}

	void MovePassengers(bool beforeMovePlatform){
		foreach(PassengerMovement  passenger  in passengerMovement){
			if(!passengerDictionary.ContainsKey(passenger.transform)){
				passengerDictionary.Add(passenger.transform ,  passenger.transform.GetComponent<Controller2D>());
			}
			if(passenger.moveBeforePlatform == beforeMovePlatform){
				passengerDictionary[passenger.transform].Move(passenger.velocity , passenger.standingOnPlatform);
			}
		}
	}

	void CalculatePassengerMovement(Vector3 velocity){
		HashSet<Transform> movedPassengers = new HashSet<Transform>();
		passengerMovement = new List<PassengerMovement>();
		float directionX = Mathf.Sign(velocity.x);
		float directionY = Mathf.Sign(velocity.y);

		//Vertically moving platform
		if(velocity.y != 0 ){
			float rayLength = Mathf.Abs(velocity.y) + skinWidth;
			for (int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = (directionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
				rayOrigin += Vector2.right * (verticalRaySpacing * i );
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, passengerMask);
				if(hit){
					if(!movedPassengers.Contains(hit.transform)){
						float pushX = (directionY == 1)? velocity.x:0;
						float pushY = velocity.y -  (hit.distance - skinWidth) * directionY;
						movedPassengers.Add(hit.transform);
						passengerMovement.Add(new PassengerMovement(hit.transform , new Vector3(pushX,pushY), directionY == 1 , true));
					}

				}
			}	
		}
		//horizontal moving platform
		if(velocity.x != 0){
			float rayLength = Mathf.Abs(velocity.x) + skinWidth;
			for (int i = 0; i < horizontalRayCount; i++) {
				Vector2 rayOrigin = (directionX == -1)? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
				rayOrigin += Vector2.up * (horizontalRaySpacing * i);
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, passengerMask);
				if(hit){
					if(!movedPassengers.Contains(hit.transform)){
						float pushX = velocity.x -  (hit.distance - skinWidth) * directionX;
						float pushY = -skinWidth;
						passengerMovement.Add(new PassengerMovement(hit.transform , new Vector3(pushX,pushY), false , true));
						movedPassengers.Add(hit.transform);
					}

				}
			}
		}
		//Passenger of top of a horizzontally or downward moving platform
		if(directionY == -1 || velocity.y == 0 && velocity.x != 0){
			float rayLength =skinWidth * 2;
			for (int i = 0; i < verticalRayCount; i++) {
				Vector2 rayOrigin = raycastOrigins.topLeft +  Vector2.right * (verticalRaySpacing * i );
				RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up , rayLength, passengerMask);
				if(hit){
					if(!movedPassengers.Contains(hit.transform)){
						float pushX = velocity.x;
						float pushY = velocity.y;
						passengerMovement.Add(new PassengerMovement(hit.transform , new Vector3(pushX,pushY), true , false));
						movedPassengers.Add(hit.transform);
					}

				}
			}	
		}
	}
	void OnDrawGizmos(){
		if(localWaypoints != null){
			Gizmos.color = Color.red;
			float size = .3f;
			for (int i = 0; i < localWaypoints.Length; i++){
				Vector3 globalWaypointPosition = (Application.isPlaying)?globalWaypoints[i]:localWaypoints[i] + transform.position;
				Gizmos.DrawLine(globalWaypointPosition - Vector3.up * size , globalWaypointPosition + Vector3.up * size);
				Gizmos.DrawLine(globalWaypointPosition - Vector3.left * size , globalWaypointPosition + Vector3.left * size);

			}

		}
	}

	struct PassengerMovement{
		public Transform transform;
		public Vector3 velocity;
		public bool  standingOnPlatform;
		public bool moveBeforePlatform;
		 public PassengerMovement(Transform _transform , Vector3 _velocity , bool _standingOnPlatform , bool _moveBeforePlatform) 
		{
			this.transform = _transform;
			this.standingOnPlatform = _standingOnPlatform;
			this.velocity = _velocity;
			this.moveBeforePlatform = _moveBeforePlatform;	
		}
	}
}
