using UnityEngine;
using UnityEngine.SceneManagement;

public class NextSceneLoader : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Make sure your player GameObject has the tag "Player"
        {
            // Load next scene by name
<<<<<<< HEAD
            SceneManager.LoadScene("Stage 2");
=======
            SceneManager.LoadScene("Stage2");
>>>>>>> 268aa96d15f71a08855df4baaec8da83e9344ca6
            
            // Or load next scene by build index:
            // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
