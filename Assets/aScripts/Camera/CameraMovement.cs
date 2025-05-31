using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private float cameraSpeed = 1.0f;

    [SerializeField]
    private float cameraLimit = 20;

    private bool lockCamera = false;
    private bool moveCamera = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            lockCamera = !lockCamera;
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            moveCamera = true;
        }
        else if (Input.GetKeyUp(KeyCode.Space))
        {
            moveCamera = false;
        }

        transform.position = player.transform.position;

        //if (lockCamera)
        //{
        //    if (!moveCamera)
        //    {
        //        transform.position = player.transform.position;
        //    }
        //    else if (moveCamera)
        //    {
        //        moveCameraWithMouse();
        //    }
        //}
        //else if (!lockCamera)
        //{
        //    if (!moveCamera)
        //    {
        //        moveCameraWithMouse();
        //    }
        //    else if (moveCamera)
        //    {
        //        transform.position = player.transform.position;
        //    }
        //}
    }

    void moveCameraWithMouse()
    {
        float maxX = player.transform.position.x + cameraLimit;
        float minX = player.transform.position.x - cameraLimit;
        float maxZ = player.transform.position.z + cameraLimit;
        float minZ = player.transform.position.z - cameraLimit;

        // Lấy tọa độ của con trỏ chuột
        Vector3 mousePosition = Input.mousePosition;

        // Lấy kích thước của màn hình game (width và height)
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        Vector3 cameraMovement = Vector3.zero; // Tạo vector để di chuyển camera

        // Kiểm tra nếu con trỏ chuột vượt quá màn hình và di chuyển camera tương ứng
        if (mousePosition.x < 0) // Vượt khỏi màn hình bên trái
        {
            cameraMovement.x = -1;
        }
        else if (mousePosition.x > screenWidth) // Vượt khỏi màn hình bên phải
        {
            cameraMovement.x = 1;
        }

        if (mousePosition.y < 0) // Vượt khỏi màn hình bên dưới
        {
            cameraMovement.z = -1;
        }
        else if (mousePosition.y > screenHeight) // Vượt khỏi màn hình bên trên
        {
            cameraMovement.z = 1;
        }

        // Tính toán vị trí mới của camera
        Vector3 newCameraPosition = transform.position + cameraMovement * cameraSpeed * Time.deltaTime;

        // Giới hạn vị trí camera trong khoảng min/max
        newCameraPosition.x = Mathf.Clamp(newCameraPosition.x, minX, maxX);
        newCameraPosition.z = Mathf.Clamp(newCameraPosition.z, minZ, maxZ);


        // Cập nhật vị trí mới cho camera
        transform.position = newCameraPosition;
    }
}
