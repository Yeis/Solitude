using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine.Unity;

public class PlayerAnimationController : MonoBehaviour
{
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

	private void Awake()
	{
        rigidBody = GetComponent<Rigidbody2D> ();
        skeletonAnimation = GetComponentInChildren<SkeletonAnimation> ();
		skeletonAnimation.SkeletonDataAsset.defaultMix = defaultMix;
	}

	// Use this for initialization
	private void Start ()
	{
		
	}
	
	// Update is called once per frame
	private void Update ()
	{
        if (IsJumping)
        {
            SetAnimation("jump", 1f, false);
        }
        else if (IsFalling)
        {
            SetAnimation("fall", 1f, false);
        }
		else if (IsMoving)
		{
            SetAnimation("run", runAnimationScale, true);
		} 
		else
		{
            SetAnimation("idle", idleAnimationScale, true);
		}
	}

    /// <summary>
    /// Sets the currently playing animation
    /// </summary>
    /// <param name="animationName"></param>
    /// <param name="timeScale"></param>
    /// <param name="loop"></param>
    private void SetAnimation(string animationName, float timeScale, bool loop)
    {
        skeletonAnimation.AnimationName = animationName;
        skeletonAnimation.timeScale = timeScale;
        skeletonAnimation.loop = loop;
    }
}
