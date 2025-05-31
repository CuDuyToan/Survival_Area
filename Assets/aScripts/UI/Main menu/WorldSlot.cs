using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WorldSlot : MonoBehaviour
{
    private string worldName = "world";

    private string fullPath = "";

    [SerializeField] public GameObject playButton;

    public void EnterPath(string path)
    {
        fullPath = path;
    }

    private string instanceID = "";


    public void GetID(string id)
    {
        instanceID = id;
    }

    [SerializeField] private Text worldName_text;
    public string WorldName
    {
        set
        {
            worldName = value;

            worldName_text.text = worldName;
        }
        get
        {
            return worldName;
        }
    }

    [SerializeField] private Text totalTime_text;
    private int totalPlaytime;
    public int TotalPlayTime
    {
        set 
        {
            totalPlaytime = value;
            int hours = (int)(value / 3600);
            int minutes = (int)((value % 3600) / 60);
            int seconds = (int)(value % 60);

            totalTime_text.text = string.Format("{0:D2}:{1:D2}:{2:D2}", hours, minutes, seconds);

        }
        get
        {
            return totalPlaytime;
        }
    }

    private float timeInGame;
    public float TimeInGame
    {
        set
        {
            timeInGame = value;
        }
        get
        {
            return timeInGame;
        }
    }

    private void OnEnable()
    {
        OnClickSlot += Hide;
    }

    private void OnDisable()
    {
        OnClickSlot -= Hide;
    }

    [Header("hide group")]
    [SerializeField] private GameObject hideGroup;
    public static event Action OnClickSlot;

    private void Hide()
    {
        hideGroup.SetActive(false);
    }

    public void OnClickWorld()
    {
        OnClickSlot?.Invoke();
        if (instanceID == "") return;

        hideGroup.SetActive(true);

        SaveAndLoadSystem.worldID = this.instanceID;
    }
    public static event Action<string> OnPlayGame;
    public void OnClickPlay()
    {
        if (instanceID == "") return;

        SaveAndLoadSystem.totalPlayTime = TotalPlayTime;
        SaveAndLoadSystem.timeInGame = TimeInGame;

        OnPlayGame?.Invoke("GamePlay");
    }

    public void OnClickDelete()
    {
        if (instanceID != "")
        {
            string path = SaveAndLoadSystem.SavePath;

            if (File.Exists(path))
            {
                File.Delete(path);
                Debug.Log("Đã xoá file save: " + path);
            }
            else
            {
                Debug.LogWarning("Không tìm thấy file để xoá: " + path);
            }
        }

        Destroy(this.gameObject);
    }

    public void OnClickRename()
    {
        Debug.Log("Rename");
        StartCoroutine(Rename());
    }

    private IEnumerator Rename()
    {
        Color oldColor = this.worldName_text.color;
        this.worldName_text.color = Color.white;

        string oldName = this.WorldName;
        this.WorldName = "";

        //string path = $"{savePath}/world_{instantID}.binary";

        while (true)
        {
            if (Input.GetMouseButton(1) || Input.GetMouseButton(0) || Input.GetKey(KeyCode.Return))
            {
                break;
            }

            if(Input.inputString != "")
            {
                foreach (char c in Input.inputString)
                {
                    //Backspace
                    if (c == '\b')
                    {
                        if (this.WorldName.Length > 0)
                            this.WorldName = this.WorldName.Substring(0, this.WorldName.Length - 1);
                    }
                    //Enter
                    else if (c == '\n' || c == '\r')
                    {
                        Debug.Log("Tên thế giới đã nhập: " + this.WorldName);
                        break;
                    }
                    else
                    {
                        this.WorldName += c;
                    }
                }
            }
            yield return null;
        }

        if (this.WorldName == "") this.WorldName = oldName;

        this.worldName_text.color = oldColor;

        BinaryFormatter formatter = new BinaryFormatter();

        WorldData data;
        using (FileStream stream = new FileStream(fullPath, FileMode.Open))
        {
            data = formatter.Deserialize(stream) as WorldData;
        }

        data.worldName = this.WorldName;

        using (FileStream stream = new FileStream(fullPath, FileMode.Create))
        {
            formatter.Serialize(stream, data);
        }


        yield return null;
    }

}
