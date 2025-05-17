using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem; // Add this for new Input System
using PlayerController;

public class PlatformPassThrough : MonoBehaviour
{
    private Collider2D _collider;
    private bool _playerOnPlatform;
    private GameObject _currentPlayer; // Keep track of the player object

    [SerializeField] private string playerTag = "Player"; // Configurable player tag

    private void Start()
    {
        _collider = GetComponent<Collider2D>();  // Platform's collider
    }

    // This method will be called by the Player's Input System
    public void OnDownInput(InputAction.CallbackContext context)
    {
        // Check if down was pressed and the player is on the platform
        if (_playerOnPlatform && context.performed)
        {
            PassThroughPlatform();
        }
    }

    private void PassThroughPlatform()
    {
        if (_currentPlayer != null)
        {
            // Temporarily disable the player's collider to allow passing through
            Collider2D playerCollider = _currentPlayer.GetComponent<Collider2D>();
            if (playerCollider != null)
                playerCollider.enabled = false;

            // Start coroutine to re-enable the player's collider after a short delay
            StartCoroutine(EnablePlayerCollider(playerCollider));
        }
    }

    private IEnumerator EnablePlayerCollider(Collider2D playerCollider)
    {
        yield return new WaitForSeconds(0.5f);  // Wait for 0.5 seconds
        // Re-enable the player's collider
        if (playerCollider != null)
            playerCollider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<Player2DPlatformController>();
        if (player != null)
        {
            _playerOnPlatform = value;
            if (value)
            {
                _currentPlayer = other.gameObject;
            }
            else if (_currentPlayer == other.gameObject)
            {
                _currentPlayer = null;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, false);  // Player leaves the platform
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);  // Player is on the platform
    }
}