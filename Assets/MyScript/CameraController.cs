using UnityEngine;

public class CameraController : MonoBehaviour
{
    GameObject player;
    private float currentCameraRotationX = 0;
    private float currentCameraRotationY = 0;
    float sensivity = 3.0f;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        MouseInputMovement();
    }

    void MouseInputMovement()
    {
        float yRotateSize = Input.GetAxis("Mouse X");
        float yRotate = yRotateSize * sensivity;
        currentCameraRotationY += yRotate;

        float xRotateSize = Input.GetAxis("Mouse Y");
        float xRotate = -xRotateSize * sensivity;
        currentCameraRotationX += xRotate;
        currentCameraRotationX = Mathf.Clamp(currentCameraRotationX, -45, 60);
        transform.localEulerAngles = new Vector3(currentCameraRotationX, currentCameraRotationY, 0);

        player.transform.Rotate(new Vector3(0, yRotate, 0));

        float rad = Mathf.Deg2Rad * currentCameraRotationY;
        float x = 0.7f * Mathf.Sin(rad);
        float z = 0.7f * Mathf.Cos(rad);

        //Vector3 position = player.transform.position + new Vector3(x, 1.2f, z);
        //transform.position = position;
    }
}