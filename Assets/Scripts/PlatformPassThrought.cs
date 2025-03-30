using System.Collections;
using UnityEngine;
using PlayerController;

public class PlatformPassThrought : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private Collider2D _collider;
    private bool _playerOnPlatform;

    private void Start()
    {
        _collider = GetComponent<Collider2D>();
    }

    private void Update()
    {
        if(_playerOnPlatform && Input.GetAxisRaw("Vertical") < 0)
        {
            _collider.enabled = false;
            StartCoroutine(EnableCollider());
        }
    }

    private IEnumerator EnableCollider()
    {
        yield return new WaitForSeconds(0.5f);
        _collider.enabled = true;
    }

    private void SetPlayerOnPlatform(Collision2D other, bool value)
    {
        var player = other.gameObject.GetComponent<Player2DPlatformController>();
        if(player != null){
            _playerOnPlatform = value;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        SetPlayerOnPlatform(other, true);
    }
}
