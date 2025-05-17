using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using PlayerController;

//Uses the collider to check directions to see if the object is currently on the ground, touching the wall, or touching the ceiling
public class TouchingDirections : MonoBehaviour
{
    public ContactFilter2D castFilter;
    public float groundDistance = 0.05f;
    public float wallDistance = 0.3f; // Increased from 0.2 to 0.3
    public float ceilingDistance = 0.05f;

    CapsuleCollider2D touchingCol;
    Animator animator;
    RaycastHit2D[] groundHits = new RaycastHit2D[5];
    RaycastHit2D[] wallHits = new RaycastHit2D[5];
    RaycastHit2D[] ceilingHits = new RaycastHit2D[5];

    [SerializeField]
    private bool _isOnWall;

    public bool IsOnWall { get 
    {
        return _isOnWall;
    }
    private set
    {
        _isOnWall = value;
        animator.SetBool(AnimationStrings.isOnWall, value);
    } 
    }

    [SerializeField]
    private bool _isOnCeiling;

    public bool IsOnCeiling { get 
    {
        return _isOnCeiling;
    }
    private set
    {
        _isOnCeiling = value;
        animator.SetBool(AnimationStrings.isOnCeiling, value);
    } 
    }

    [SerializeField]
    private bool _isGrounded;
    
    // Modified to handle small scales better by using >= 0 instead of > 0
    private Vector2 wallCheckDirection => gameObject.transform.localScale.x >= 0 ? Vector2.right : Vector2.left;

    public bool IsGrounded { get 
    {
        return _isGrounded;
    }
    private set
    {
        _isGrounded = value;
        animator.SetBool(AnimationStrings.isGrounded, value);
    } 
    }

    private void Awake()
    {
        touchingCol = GetComponent<CapsuleCollider2D>();
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        IsGrounded = touchingCol.Cast(Vector2.down, castFilter, groundHits, groundDistance) > 0;
        IsOnWall = touchingCol.Cast(wallCheckDirection, castFilter, wallHits, wallDistance) > 0;
        IsOnCeiling = touchingCol.Cast(Vector2.up, castFilter, ceilingHits, ceilingDistance) > 0;
    }

    // Added visualization for debugging
    private void OnDrawGizmos()
    {
        if (touchingCol == null) return;
        
        // Debug ground check
        Gizmos.color = IsGrounded ? Color.green : Color.red;
        Vector2 groundCheckPosition = (Vector2)transform.position + new Vector2(0, -groundDistance);
        Gizmos.DrawLine(transform.position, groundCheckPosition);
        
        // Calculate wall check direction for gizmos
        Vector2 wallDir = transform.localScale.x >= 0 ? Vector2.right : Vector2.left;
        
        // Debug wall check
        Gizmos.color = IsOnWall ? Color.blue : Color.yellow;
        Vector2 wallCheckPosition = (Vector2)transform.position + new Vector2(wallDir.x * wallDistance, 0);
        Gizmos.DrawLine(transform.position, wallCheckPosition);
    }
}