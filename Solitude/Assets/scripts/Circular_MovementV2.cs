using System.Collections;
using System.Collections.Generic;
using UnityEngine;
 
 public class Circular_MovementV2 : MonoBehaviour 
 {
 
     public float RotateSpeed = 5f;
     public float Radius = 0.1f;
 
     private Vector3 _centre;
     public float _angle;
 
     private void Start()
     {
         _centre = transform.position;
     }
 
     private void Update()
     {
 
         _angle += RotateSpeed * Time.deltaTime;
 
         var offset = new Vector3(Mathf.Sin(_angle), Mathf.Cos(_angle),0.0f) * Radius;
         transform.position = _centre + offset;
     }
  
  
 
 }