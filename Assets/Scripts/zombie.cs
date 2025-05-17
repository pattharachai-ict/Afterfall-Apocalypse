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
<<<<<<< HEAD
    public DetectionZone cliffDetectionZone;
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
    AudioManager audioManager;
    
    // Add a longer cooldown and state tracking
    private float lastFlipTime = 0f;
    private float flipCooldown = 1f; // Increased cooldown time
    private bool isProcessingCliffDetection = false;
    private Vector2 lastPosition;
    private float stuckThreshold = 0.1f; // Distance threshold to consider "stuck"
    private float stuckCheckTime = 0.5f;
    private float lastStuckCheckTime = 0f;
=======
    public DetectionZone cliffDetectionZone; // Add reference to cliff detection zone
    Rigidbody2D rb;
    TouchingDirections touchingDirections;
    Animator animator;
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    public enum WalkableDirection { Right, Left }

    private WalkableDirection _walkDirection = WalkableDirection.Right;
    private Vector2 WalkDirectionVector = Vector2.right;

    public WalkableDirection WalkDirection
    {
        get { return _walkDirection; }
        set {
<<<<<<< HEAD
            if (_walkDirection != value)
            {
                float newXScale = Mathf.Abs(gameObject.transform.localScale.x);
                if (value == WalkableDirection.Left)
                    newXScale = -newXScale;

                gameObject.transform.localScale = new Vector2(newXScale, gameObject.transform.localScale.y);
                WalkDirectionVector = (value == WalkableDirection.Right) ? Vector2.right : Vector2.left;
            }

            _walkDirection = value;
=======
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
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        }
    }

    public bool _hasTarget = false;
<<<<<<< HEAD
    public bool HasTarget
    {
        get { return _hasTarget; }
        private set
        {
            if (_hasTarget != value && value == true)
            {
                audioManager.PlaySFX(audioManager.zombie);
            }

            _hasTarget = value;
            animator.SetBool(AnimationStrings.hasTarget, value);
        }
    }

    public bool CanMove
    {
=======
    public bool HasTarget { get{return _hasTarget;} private set
    {
        _hasTarget = value;
        animator.SetBool(AnimationStrings.hasTarget, value);
    }
     }

     public bool CanMove
     {
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        get
        {
            return animator.GetBool(AnimationStrings.canMove);
        }
<<<<<<< HEAD
    }
=======
     }
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        touchingDirections = GetComponent<TouchingDirections>();
        animator = GetComponent<Animator>();
<<<<<<< HEAD
        lastPosition = transform.position;

        // Initialize AudioManager
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManager>();
    }

    void Update()
    {
        HasTarget = attackZone.DetectedColliders.Count > 0;

        // Check if zombie is stuck by not moving much over time
        if (Time.time > lastStuckCheckTime + stuckCheckTime)
        {
            float distanceMoved = Vector2.Distance(transform.position, lastPosition);
            if (distanceMoved < stuckThreshold && touchingDirections.IsGrounded)
            {
                // If stuck, force a direction change
                if (Time.time > lastFlipTime + flipCooldown)
                {
                    FlipDirection();
                    lastFlipTime = Time.time;
                }
            }
            lastPosition = transform.position;
            lastStuckCheckTime = Time.time;
        }

        // Only process cliff detection if we're not currently processing it
        if (!isProcessingCliffDetection && Time.time > lastFlipTime + flipCooldown)
        {
            // Check for cliff or wall conditions
            if (cliffDetectionZone != null && 
                cliffDetectionZone.DetectedColliders.Count == 0 && 
                touchingDirections.IsGrounded)
            {
                StartCoroutine(ProcessCliffDetection());
            }
        }
    }

    private IEnumerator ProcessCliffDetection()
    {
        isProcessingCliffDetection = true;
        
        // Wait a small amount of time to confirm this is really a cliff
        // This prevents false positives from momentary detection failures
        yield return new WaitForSeconds(0.1f);
        
        // Double-check that we're still at a cliff
        if (cliffDetectionZone != null && 
            cliffDetectionZone.DetectedColliders.Count == 0 && 
            touchingDirections.IsGrounded)
        {
            FlipDirection();
            lastFlipTime = Time.time;
        }
        
        isProcessingCliffDetection = false;
    }

    private void FixedUpdate()
    {
        if (touchingDirections.IsGrounded && touchingDirections.IsOnWall && Time.time > lastFlipTime + flipCooldown)
        {
            FlipDirection();
            lastFlipTime = Time.time;
        }

        if (CanMove)
            rb.linearVelocity = new Vector2(walkSpeed * WalkDirectionVector.x, rb.linearVelocity.y);
        else
            rb.linearVelocity = new Vector2(Mathf.Lerp(rb.linearVelocity.x, 0, walkStopRate), rb.linearVelocity.y);
=======
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
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
    }

    private void FlipDirection()
    {
<<<<<<< HEAD
        if (WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        }
=======
        if(WalkDirection == WalkableDirection.Right)
        {
            WalkDirection = WalkableDirection.Left;
        }  
        else if (WalkDirection == WalkableDirection.Left)
        {
            WalkDirection = WalkableDirection.Right;
        } 
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
        else
        {
            Debug.LogError("Current walkable direction is not set to legal values of right or left");
        }
    }
}