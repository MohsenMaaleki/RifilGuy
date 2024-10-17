using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public Toggle togglelock;
    void Start()
    {
        if (PlayerPrefs.GetInt("auto") == 1)
        {
            togglelock.isOn = true;
        }
        else
        {
            togglelock.isOn = false;
        }
    }
    public void PlayGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
    

}
