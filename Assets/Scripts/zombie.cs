using System;
using System.Collections;
using System.Collections.Generic;
using PlayerController;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D), typeof(TouchingDirections))]
public class zombie : MonoBehaviour
{
    public float walkSpeed = 3f;
    public float walkStopRate = 0.05f;
    public DetectionZone attackZone;
    public DetectionZone cliffDetectionZone; // Add reference to cliff detection zone
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
            if(_walkDirection != value) 
            {
                // Direction flipped - don't use scale multiplication for small objects
                float newXScale = Mathf.Abs(gameObject.transform.localScale.x);
                if (value == WalkableDirection.Left)
                    newXScale = -newXScale;  
                    
                gameObject.transform.localScale = new Vector2(
                    newXScale, 
                    gameObject.transform.localScale.y
                );

                // Set direction vector
                WalkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }
            
            _walkDirection = value; 
        }
    }

    public bool _hasTarget = false;
    public bool HasTarget { get{return _hasTarget;} private set
    {
        _hasTarget = value;
        animator.SetBool(AnimationStrings.hasTarget, value);
    }
     }

     public bool CanMove
     {
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
     }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        HasTarget = attackZone.DetectedColliders.Count > 0;
        
        // Check if approaching a cliff (no ground detected)
        if (cliffDetectionZone != null && cliffDetectionZone.DetectedColliders.Count == 0 && touchingDirections.IsGrounded)
        {
            FlipDirection();
        }
    }
    
    private void FixedUpdate()
    {
        // Only flip direction if we're on the ground AND on a wall
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall)
        {
            FlipDirection();
        }

        if(CanMove)
            rb.linearVelocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x,0, walkStopRate), rb.linearVelocity.y);
    }

    private void FlipDirection()
    {
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }  
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } 
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }
}