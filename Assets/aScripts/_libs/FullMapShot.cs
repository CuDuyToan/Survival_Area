using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FullMapShot : MonoBehaviour
{
    public Camera captureCamera;
    public RenderTexture renderTexture;

    private void Awake()
    {
        captureCamera = GetComponent<Camera>();
        renderTexture = captureCamera.targetTexture;
    }

    public void CaptureToTexture()
    {
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = renderTexture;

        captureCamera.targetTexture = renderTexture;
        captureCamera.Render();

        Texture2D image = new Texture2D(renderTexture.width, renderTexture.height, TextureFormat.RGB24, false);
        image.ReadPixels(new Rect(0, 0, renderTexture.width, renderTexture.height), 0, 0);
        image.Apply();

        RenderTexture.active = currentRT;
        captureCamera.targetTexture = null;

        // Lưu ảnh dưới dạng PNG
        byte[] bytes = image.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/MapCapture.png", bytes);

        Debug.Log("Captured and saved!");
    }

    private void OnEnable()
    {
        CaptureToTexture();
    }

}
