using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    [SerializeField] private string nextSceneName;
    [SerializeField] private bool showDebugLogs = true;
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the player entered the trigger
        if (collision.CompareTag("Player"))
        {
            TransitionToNextScene();
        }
    }
    
    // Extracted method for scene transition logic
    private void TransitionToNextScene()
    {
        // Find the PlayerStateManager and save the state
        PlayerStateManager stateManager = FindObjectOfType<PlayerStateManager>();
        if (stateManager != null)
        {
            stateManager.SavePlayerState();
            if (showDebugLogs) Debug.Log("Player state saved before scene transition");
        }
        else
        {
            Debug.LogWarning("PlayerStateManager not found! State will not be saved.");
        }
        
        // Load the next scene
        if (showDebugLogs) Debug.Log($"Loading next scene: {nextSceneName}");
        SceneManager.LoadScene(nextSceneName);
    }
    
    // You can also use this method to manually trigger scene transition from UI button or other events
    public void LoadNextSceneManually()
    {
        TransitionToNextScene();
    }
}