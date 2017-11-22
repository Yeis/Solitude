using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerAnimationController : MonoBehaviour {

	private Rigidbody2D rigidBody;
	private SkeletonAnimation skeletonAnimation;

	[SerializeField] float defaultMix = 1f;
	[SerializeField] float idleAnimationScale = 1f;
	[SerializeField] float runAnimationScale = 1f;

	private bool IsJumping
	{
		get { return rigidBody.velocity.y > 0f; }
	}

	private bool IsFalling
	{
		get { return rigidBody.velocity.y < 0f; }
	}

	private bool IsMoving
	{
		get { return rigidBody.velocity.x != 0f; }
	}

	void Awake()
	{
		this.rigidBody = GetComponent<Rigidbody2D> ();
		this.skeletonAnimation = GetComponentInChildren<SkeletonAnimation> ();
		this.skeletonAnimation.SkeletonDataAsset.defaultMix = defaultMix;
	}

	// Use this for initialization
	void Start ()
	{
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (IsMoving)
		{
			skeletonAnimation.AnimationName = "run";
			skeletonAnimation.timeScale = runAnimationScale;
		} 
		else
		{
			skeletonAnimation.AnimationName = "idle";
			skeletonAnimation.timeScale = idleAnimationScale;
		}
	}
}
