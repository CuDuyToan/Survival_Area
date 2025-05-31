using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameMenu : MonoBehaviour
{
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LoadScene loadScene;
    public void OnClickMainMenu()
    {
        saveManager.SaveGame();
        //SceneManager.LoadScene("MainScene");
        loadScene.SceneName = "MainScene";
        loadScene.gameObject.SetActive(true);
    }

    public void OnClickSaveGame()
    {
        saveManager.SaveGame();
    }
}
