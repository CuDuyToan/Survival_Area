using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    #region data

    private List<string> listWorld = new List<string>();

    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        System.Random random = new System.Random();
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }

    private string GetNewInstanceID()
    {
        string ID = RandomString(5);

        foreach (string fileWorld in listWorld)
        {
            if ($"world_{ID}.binary" == fileWorld)
            {
                ID = GetNewInstanceID();
            }
        }

        return ID;
    }

    private void LoadDataFile()
    {
        string fullPath = SaveAndLoadSystem.folderPath;

        if (Directory.Exists(fullPath))
        {
            string[] binaryFiles = Directory.GetFiles(fullPath, "*.binary");

            foreach (string file in binaryFiles)
            {
                string fileName = Path.GetFileName(file);

                listWorld.Add(fileName);

                Debug.Log("Tìm thấy file: " + fileName);

                CreateWorldSlot(file, fileName);
            }
        }
        else
        {
            Debug.LogWarning("Thư mục chưa tồn tại: " + fullPath);
        }
    }

    private void CreateWorldSlot(string filePath, string fileName)
    {
        FileInfo fileInfo = new FileInfo(filePath);
        if (fileInfo.Length > 0)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(filePath, FileMode.Open);
            WorldData data = formatter.Deserialize(stream) as WorldData;

            string ID = fileName.Replace("world_", "").Replace(".binary", "");


            GameObject world = Instantiate(this.worldSlot_prefab, loadWorld_content);

            WorldSlot slot = world.GetComponent<WorldSlot>();

            slot.EnterPath(filePath);
            slot.WorldName = data.worldName;

            slot.TotalPlayTime = (int)data.totalPlayTime;
            slot.TimeInGame = (int)data.timeInGame;
            slot.GetID(ID);

            slot.playButton.SetActive(!data.end);


            stream.Close();
        }
        else
        {
            Debug.LogWarning("File trống: " + filePath);
        }        
    }

    #endregion

    #region menu

    [SerializeField] private GameObject home;

    [Header("Load world")]
    [SerializeField] private GameObject loadWorld;
    [SerializeField] private Transform loadWorld_content;
    [SerializeField] private GameObject worldSlot_prefab;

    #endregion menu

    private void Awake()
    {
        LoadDataFile();
    }

    private void OnEnable()
    {
        WorldSlot.OnPlayGame += LoadScene;
    }

    private void OnDisable()
    {
        WorldSlot.OnPlayGame -= LoadScene;
    }

    public void OnClickHome()
    {
        this.home.SetActive(true);

        this.loadWorld.SetActive(false);
    }

    public void OnClickLoadGame()
    {
        this.loadWorld.SetActive(true);

        this.home.SetActive(false);
    }

    #region new game
    public void OnClickNewGame()
    {
        SaveAndLoadSystem.worldID = GetNewInstanceID();

        SaveAndLoadSystem.timeInGame = 60 * 60 * 5;
        SaveAndLoadSystem.totalPlayTime = 0;

        LoadScene("GamePlay");
    }

    #endregion new game



    public void OnExitGame()
    {
        Application.Quit();
    }


    [Header("Load scene UI")]
    [SerializeField] private LoadScene LoadingScene;
    [SerializeField] private List<GameObject> hideOnLoading;

    private void LoadScene(string sceneName)
    {
        if(hideOnLoading.Count >0)
        {
            foreach (GameObject hideItem in hideOnLoading)
            {
                hideItem.SetActive(false);
            }
        }
        LoadingScene.SceneName = sceneName;
        LoadingScene.gameObject.SetActive(true);
    }


}
