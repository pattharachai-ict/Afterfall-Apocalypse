using UnityEngine;
using UnityEngine.SceneManagement;

public class SettingMenu : MonoBehaviour
{
    [SerializeField] GameObject settingMenu;
  
    public void Pause()
    {
        settingMenu.SetActive(true);
        Time.timeScale = 0;
    }
    public void Home()
    {
      SceneManager.LoadScene("Main Menu");
      Time.timeScale = 1;
    }

    public void Close()
    {
       settingMenu.SetActive(false);
       Time.timeScale = 1;
    }
}