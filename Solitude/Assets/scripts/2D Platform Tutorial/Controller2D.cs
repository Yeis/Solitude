﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Controller2D : RayCastController {


    public CollisionInfo collisions;

    float maxClimbAngle =80;
    float maxDescendingAngle = 75; 

	// Use this for initialization
    public override void Start(){
        base.Start();
    }
	
	// Update is called once per frame
    public void Move(Vector3 velocity , bool standingOnPlatform = false) {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.velocityOld = velocity; 

        if(velocity.y < 0){
            DescendSlope(ref velocity);
        }

        //handle collisions
        if(velocity.x != 0){
            HorizontalCollisions( ref velocity);
        }
        if(velocity.y != 0){
            VerticalCollisions( ref velocity);
        }
        transform.Translate(velocity);

        if(standingOnPlatform){
            collisions.below = true;
        }
    }


    void HorizontalCollisions(ref Vector3 velocity){
        float directionX = Mathf.Sign(velocity.x);
        float rayLength = Mathf.Abs(velocity.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++) {
            Vector2 rayOrigin = (directionX == -1)? raycastOrigins.bottomLeft : raycastOrigins.bottomRight;
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red);            

            if (hit){
                if(hit.distance == 0){
                    continue;
                }
                float slopeAngle  = Vector2.Angle(hit.normal ,Vector2.up);

                if(i == 0 && slopeAngle <= maxClimbAngle){
                    if(collisions.descendingSlope){
                        collisions.descendingSlope = false;
                        velocity = collisions.velocityOld;
                    }
                    float distanceToSlopeStart = 0;
                    if(slopeAngle != collisions.slopeAngleOld){
                        distanceToSlopeStart= hit.distance - skinWidth;
                        velocity.x -= distanceToSlopeStart * directionX;
                    }
                    ClimbSlope(ref velocity , slopeAngle);
                    velocity.x += distanceToSlopeStart * directionX;

                }
                if(!collisions.climbingSlope ||  slopeAngle > maxClimbAngle ){
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;
                    if(collisions.climbingSlope){
                        velocity.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);
                    }
                    collisions.left = directionX == -1;
                    collisions.right = directionX == 1;
                }
            } 
        }
    } 

    void ClimbSlope(ref Vector3 velocity  , float slopeAngle){
        float moveDistance =   Mathf.Abs(velocity.x);
        float climbVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        if(velocity.y <=  climbVelocityY){
            velocity.y = climbVelocityY;
            velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle; 
        }
    }

    void DescendSlope(ref Vector3 velocity){
        float directionX = Mathf.Sign(velocity.x);
        Vector2 rayOrigin  = (directionX == -1 )? raycastOrigins.bottomRight:raycastOrigins.bottomLeft;
        RaycastHit2D hit  = Physics2D.Raycast(rayOrigin , -Vector2.up , Mathf.Infinity , collisionMask);
        if(hit){
            float slopeAngle = Vector2.Angle(hit.normal , Vector2.up);
            if(slopeAngle != 0 && slopeAngle <= maxDescendingAngle){
                if(Mathf.Sign(hit.normal.x) == directionX){
                    if(hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x)){
                        float moveDistance = Mathf.Abs(velocity.x);
                        float descendingVelocityY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        velocity.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(velocity.x);
                        velocity.y -= descendingVelocityY;

                        collisions.below = true;
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;  
                    }
                }
            }
        }
 
    }
 
    void VerticalCollisions(ref Vector3 velocity) {
        float directionY = Mathf.Sign(velocity.y);
        float rayLength = Mathf.Abs(velocity.y) + skinWidth;

        for (int i = 0; i < verticalRayCount; i++) {
            Vector2 rayOrigin = (directionY == -1)? raycastOrigins.bottomLeft : raycastOrigins.topLeft;
            rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red);

            if (hit){
                velocity.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                if(collisions.climbingSlope){
                    velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(velocity.x);

                }
                collisions.below = directionY == -1;
                collisions.above = directionY == 1;
            }
            
        }
        if(collisions.climbingSlope){
            float directionX = Mathf.Sign(velocity.x);
            rayLength =  Mathf.Abs(velocity.x) + skinWidth;
            Vector2 rayOrigin = ((directionX == -1)? raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin,Vector2.right * directionX , rayLength , collisionMask);
            if(hit){
                float slopeAngle = Vector2.Angle(hit.normal , Vector2.up);
                if(slopeAngle != collisions.slopeAngle){
                    velocity.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle; 
                }
            }

        }

    }

    public struct CollisionInfo
    {
        public bool above, below;
        public bool left, right;
        public bool climbingSlope;
        public float slopeAngle , slopeAngleOld;
        public bool descendingSlope;
        public Vector3 velocityOld;

        public void Reset()
        {
            above = below = false;
            left = right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld =  slopeAngle;
            slopeAngle = 0; 
        }
    }
}
