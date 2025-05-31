using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadScene : MonoBehaviour
{
    [Header("Next scene")]
    [SerializeField] private string sceneName = "";
    public string SceneName
    {
        set
        {
            sceneName = value;
        }
        get
        {
            return sceneName;
        }
    }

    [Header("Display")]
    [SerializeField] private Image loadingValue;

    private void OnEnable()
    {
        StartCoroutine(LoadAsync(SceneName));
    }

    private IEnumerator LoadAsync(string sceneName)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);

        // Optionally, show a loading screen here

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            //Debug.Log("Loading progress: " + progress);
            loadingValue.fillAmount = progress;
            yield return null;
        }
    }
}
