using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    Info info;
    Scene scene;
    GameObject player;

    private void Start()
    {
        scene = SceneManager.GetActiveScene();
        player = GameObject.Find("Player");
        info = GameObject.Find("Info").GetComponent<Info>();
        Debug.Log("Active Scene name is: " + scene.name + "\nActive Scene index: " + scene.buildIndex);
    }

    public void LoadNextScene()
    {
        info.previousScene = scene.buildIndex;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void LoadStartMenu()
    {
        info.previousScene = scene.buildIndex;
        SceneManager.LoadScene(0);
    }

    public void LoadSceneFromDirectReference(int sceneIndex)
    {
        info.previousScene = scene.buildIndex;
        SceneManager.LoadScene(sceneIndex);
    }

    public void LoadPreviousScene()
    {
        int i = info.previousScene;
        info.previousScene = scene.buildIndex;
        SceneManager.LoadScene(i);
    }

    public void ResetLevel()
    {
        // directly access through the buttons loading scenes
        info.ResetInventory();
        info.isSetPosition = false;
    }

    
    public void SavePlayerPosition()
    {
        if (player)
        {
            info.SetPlayerPosition(player.transform.position);
        }
        info.isSetPosition = true;
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
