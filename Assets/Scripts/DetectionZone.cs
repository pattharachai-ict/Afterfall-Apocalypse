// Modified DetectionZone.cs for cliff detection
using System.Collections.Generic;
using UnityEngine;

public class DetectionZone : MonoBehaviour
{
    public List<Collider2D> DetectedColliders = new List<Collider2D>();
    public bool isCliffDetector = false; // Add this flag to identify cliff detectors
    Collider2D col;

    private void Awake()
    {
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // For player detection
        if (!isCliffDetector && collision.CompareTag("Player"))
        {
            DetectedColliders.Add(collision);
        }
        // For ground detection when checking for cliffs
        else if (isCliffDetector && collision.CompareTag("Ground"))
        {
            DetectedColliders.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // For player detection
        if (!isCliffDetector && collision.CompareTag("Player"))
        {
            DetectedColliders.Remove(collision);
        }
        // For ground detection when checking for cliffs
        else if (isCliffDetector && collision.CompareTag("Ground"))
        {
            DetectedColliders.Remove(collision);
        }
    }
}