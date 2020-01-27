using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void LoadLevelMenu()
    {
        SceneManager.LoadScene("LevelMenu");
    }

    public void LoadLevel(int num)
    {
        SceneManager.LoadScene("MainScene");
    }

    public void LoadStartMenu()
    {
        SceneManager.LoadScene("StartMenu");
    }

}
