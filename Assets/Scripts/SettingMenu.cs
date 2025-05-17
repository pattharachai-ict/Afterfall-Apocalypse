using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] GameObject settingMenu;
    private bool isPaused = false;

    void Update()
    {
        // Toggle pause with Escape (keyboard) or JoystickButton7 (Start button on Xbox)
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton7))
        {
            if (isPaused)
            {
                Close();
            }
            else
            {
                Pause();
            }
        }
    }

    public void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void Home()
    {
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }

    public void Close()
    {
        settingMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }
}